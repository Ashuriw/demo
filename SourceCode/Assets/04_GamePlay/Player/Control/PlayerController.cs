using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region 组件引用

    [Header("组件引用")] 
    [SerializeField] private PlayerConfig m_config;
    private Rigidbody2D m_rb;
    private Collider2D m_col;
    private SpriteRenderer m_sr;
    [HideInInspector]public Animator m_animator;
    private Sensor_Player    m_groundSensor;
    private Health m_health;
    // private Sensor_Player    m_wallSensorR1;
    // private Sensor_Player    m_wallSensorR2;
    // private Sensor_Player    m_wallSensorL1;
    // private Sensor_Player    m_wallSensorL2;
    #endregion

    #region 公共属性

    public bool IsGrounded
    {
        get
        {
            return m_grounded;
        }
    }
    public bool MovePressed
    {
        get
        {
            return m_movePressed;
        }
    }
    public int CurrentHealth
    {
        get
        {
            return m_currentHealth;
        }
    }
    public bool DorgePressed
    {
        get
        {
            return m_dorgePressed;
        }
    }
    public bool CanDorge
    {
        get
        {
            return m_canDorge;
        }
    }
    public bool AttackPressed
    {
        get
        {
            return m_attackPressed;
        }
    }

    public float ComboTimer
    {
        get
        {
            return m_config.comboTimer;
        }
    }
    public int Headed
    {
        get
        {
            return m_headed;
        }
    }

    public bool Invincible
    {
        get
        {
            return m_health.Invincible;
        }
        set
        {
            m_health.Invincible = value;
        }
    }

    public bool ParryPressed
    {
        get{return m_parryPressed;}
    }

    public bool Parry
    {
        get{return m_parry;}
        set{m_parry = value;}
    }
    #endregion

    #region 状态变量

    //状态变量
    private Dictionary<Type, PlayerStateBase> statesCache;
    private Stack<PlayerStateBase> statesStack;
    private int m_headed = 1;//取值只有-1和1，-1用来表示朝向左边，1用来表示朝向右边
    private bool m_grounded;
    private bool m_movePressed;
    private bool m_jumpPressed;
    private bool m_attackPressed;
    private bool m_dorgePressed;
    private bool m_parryPressed;
    private bool m_canDorge;
    private bool m_isInvincible;
    private float m_coyoteTimer;
    private float m_jumpBufferTimer;
    private float m_moveInput;
    private float m_nextCheckTime = -1f;
    private int m_currentHealth;
    private bool m_parry=false;

    #endregion

    #region 生命周期函数
    private void Awake()
    {
        statesStack = new Stack<PlayerStateBase>();
        
        m_rb = GetComponent<Rigidbody2D>();
        m_col = GetComponent<Collider2D>();
        m_sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        m_animator = transform.Find("Sprite").GetComponent<Animator>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
        m_health = GetComponent<Health>();
        // m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_Player>();
        // m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_Player>();
        // m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_Player>();
        // m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_Player>();
    }
    private void OnEnable()
    {
        ComeAlive();
        m_health.OnTakeDamage -= TakeDamage;
        m_health.OnTakeDamage += TakeDamage;
        m_health.OnDeath -= Die;
        m_health.OnDeath += Die;

    }

    private void OnDisable()
    {
        m_health.OnTakeDamage -= TakeDamage;
        m_health.OnDeath -= Die;
    }
    void Update()
    {
        CheckInput();
        statesStack.Peek().Update();
    }

    private void FixedUpdate()
    {
        statesStack.Peek().FixedUpdate();
    }
    #endregion

    #region 状态机控制方法
    public T GetState<T>() where T : PlayerStateBase
    {
        if(statesCache==null)statesCache = new Dictionary<Type, PlayerStateBase>();
        if (!statesCache.TryGetValue(typeof(T), out var state))
        {
            state = (T)Activator.CreateInstance(typeof(T),new  object[]{this});
            statesCache.Add(typeof(T), state);
        }
        return (T)state;
    }
    public void ChangeState(PlayerStateBase newState)
    {
        if (newState.IsSpecialState)
        {
            Debug.LogError("ChangeState 不能用于临时状态！");
            return;
        }
        statesStack.Pop().Exit();//退出旧状态
        statesStack.Push(newState);
        statesStack.Peek().Enter();//进入新状态
    }
    public void EnterSpecialState(PlayerStateBase newState)
    {
        if (!newState.IsSpecialState)
        {
            Debug.LogError("EnterSpecialState 只能用于临时状态！");
            return;
        }
        // 如果当前临时状态优先级较低，则打断
        if (IsSpecialState())
        {
            var currentState = (PlayerSpecialState)statesStack.Peek();
            var newSpecialState = (PlayerSpecialState)newState;
            if (newSpecialState.Priority >= currentState.Priority)
                statesStack.Pop().Exit();
            else
                return; // 不打断
        }
        else
            statesStack.Peek().Exit();
        statesStack.Push(newState);
        statesStack.Peek().Enter();
    }

    public void ExitSpecialState()
    {
        if (!IsSpecialState())
        {
            #if UNITY_EDITOR
            Debug.LogError("当前不在临时状态，无法 ExitSpecialState！");
            #endif
            return;
        }
        
        statesStack.Pop().Exit();
        statesStack.Peek().Enter();
    }
    private bool IsSpecialState()
    {
        return statesStack.Count > 0 && statesStack.Peek().IsSpecialState;
    }

    public void OnSpecialStateAnimationEnd()
    {
        if (!IsSpecialState())
        {
            #if UNITY_EDITOR
            Debug.LogError("当前不是临时状态，无法Invoke(OnSpecialStateAnimationEnd)");
            #endif
            return;
        }
        (statesStack.Peek() as PlayerSpecialState).OnAnimationEnd();
    }
    #endregion

    #region 辅助函数
    
    public bool IsInCombatRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            m_config.detectionRadius,
            m_config.targetLayer);

        foreach (var hit in hits)
        {
            if (hit!=null)
                return true;
        }
        return false;
    }
    

    #endregion

    #region 游戏逻辑
    /// <summary>
    /// 嘻嘻，我一定要活下去
    /// </summary>
    private void ComeAlive()
    {
        m_health.ResetHealth();
        statesStack.Push(GetState<PlayerIdleState>());
        statesStack.Peek().Enter();
        m_canDorge = true;
    }
    private void CheckInput()
    {
        m_moveInput = Input.GetAxisRaw("Horizontal");
        if (m_moveInput > 0) m_headed = 1;
        else if (m_moveInput < 0) m_headed = -1;
        m_movePressed = Mathf.Abs(m_moveInput) > 0.1f;
        m_jumpPressed = Input.GetKey(KeyCode.Space);
        m_attackPressed = Input.GetKeyDown(KeyCode.Z);
        m_dorgePressed = Input.GetKeyDown(KeyCode.LeftShift);
        m_parryPressed = Input.GetKey(KeyCode.X);
    }
    /// <summary>
    /// 你给我干哪来了，这还是地球吗？
    /// </summary>
    /// <returns></returns>
    public void CheckIsGrounded()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
        }
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
        }
    }
    public void HandleSpriteRenderer()
    {
        if (m_headed == 1)
        {
            m_sr.flipX = false;
        }
        else
        {
            m_sr.flipX = true;
        }
    }
    /// <summary>
    /// 没病走两步
    /// </summary>
    public void HandleMovement()
    {
        float targetVelocity = m_moveInput * m_config.moveSpeed;
        float currentVelocity = m_rb.velocity.x;

        // 地面移动
        if (m_grounded)
        {
            float accelerationRate = m_config.acceleration;
            if (m_moveInput == 0)
            {
                m_rb.velocity = new Vector2(
                        Mathf.MoveTowards(currentVelocity, 0, m_config.deceleration * Time.deltaTime),
                        m_rb.velocity.y);
            }
            else
            {
                m_rb.velocity = new Vector2(
                Mathf.MoveTowards(currentVelocity, targetVelocity, accelerationRate * Time.deltaTime),
                m_rb.velocity.y);
            }
        }
        // 空中移动
        else
        {
            float accelerationRate = m_config.acceleration * m_config.airControl;
            if (m_moveInput == 0)
            {
                m_rb.velocity = new Vector2(
                        Mathf.MoveTowards(currentVelocity, 0, m_config.deceleration * Time.deltaTime),
                        m_rb.velocity.y);
            }
            else
            {
                m_rb.velocity = new Vector2(
                Mathf.MoveTowards(currentVelocity, targetVelocity, accelerationRate * Time.deltaTime),
                m_rb.velocity.y);
            }
        }
    }
    public void StopMovement()
    {
        m_rb.velocity = new Vector2(0,m_rb.velocity.y);
    }
    /// <summary>
    /// 唉！小跳！
    /// </summary>
    public void HandleJump()
    {
        // Coyote Time逻辑
        if (m_grounded) m_coyoteTimer = m_config.coyoteTime;
        else m_coyoteTimer -= Time.deltaTime;

        // Jump Buffer逻辑
        if (m_jumpPressed) m_jumpBufferTimer = m_config.jumpBufferTime;
        else m_jumpBufferTimer -= Time.deltaTime;
        
        // 执行跳跃
        if (m_jumpBufferTimer > 0 && m_coyoteTimer > 0)
        {
            EnterSpecialState(GetState<PlayerJumpState>());
            m_jumpBufferTimer = 0;
            m_coyoteTimer = 0;
        }
    }
    
    /// <summary>
    /// 吴京来了都招了
    /// </summary>
    private void TakeDamage()
    {
        if (!m_parry)
        {
            EnterSpecialState(GetState<PlayerHitState>());
        }
        else
        {
            m_animator.SetTrigger("Parry");
            m_health.Parry();
        }
    }
    /// <summary>
    /// 死了啦，都是你害的啦
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DeathRoutine()); // 新增行
    }
    
    public void SpawnDustEffect(GameObject dust, float dustXOffset = 0, float dustYOffset = 0)
    {
        if (dust != null)
        {
            // 设置Dust生成位置
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_headed, dustYOffset, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // 校正x轴
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_headed, 1, 1);
        }
    }
    #endregion

    #region 协程

    public IEnumerator JumpRoutine()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, m_config.jumpForce);
        m_groundSensor.Disable(m_config.jumpDuration);
        while (m_rb.velocity.y > m_config.jumpVelocityToFall)
        {
            yield return null;
        }
        ExitSpecialState();
    }
    public IEnumerator GetHurtRoutine()
    {
        //协程用于控制时间及中断处理
        m_isInvincible = true;
        m_rb.velocity = new Vector2((-m_headed) * m_config.knockBackForce, m_config.knockBackForce / 2);
        yield return new WaitForSeconds(m_config.invincibleDuration);
        m_isInvincible = false;
        ExitSpecialState();
    }
    public IEnumerator DorgeRoutine()
    {
        // 协程用于控制冲刺时长及中断处理
        StartCoroutine(StartdorgeCoolDown());
        m_rb.velocity = new Vector2(m_config.dorgeForce * m_headed, 0);
        yield return new WaitForSeconds(m_config.dorgeDuration);
        while (Math.Abs(m_rb.velocity.x)>0.5f)
        {
            m_rb.velocity = new Vector2(
                Mathf.MoveTowards(m_rb.velocity.x, 0, m_config.deceleration * Time.deltaTime),
                m_rb.velocity.y);
            yield return null;
        }
        ExitSpecialState();
    }
    public IEnumerator StartdorgeCoolDown()
    {
        m_canDorge = false;
        yield return new WaitForSeconds(m_config.dorgeCoolDown);
        m_canDorge = true;
    }

    private IEnumerator DeathRoutine()
    {
        // 1. 播放死亡动画
        m_animator.SetTrigger("Death");
    
        // 2. 等待动画时长
        yield return new WaitForSeconds(1f);
        
        // 3. 清理状态
        while (statesStack.Count > 0)
            statesStack.Pop().Exit();
        m_canDorge = true;
        m_isInvincible = false;
    
        // 4. 失活对象
        this.gameObject.SetActive(false);
    }
    
    #endregion
}