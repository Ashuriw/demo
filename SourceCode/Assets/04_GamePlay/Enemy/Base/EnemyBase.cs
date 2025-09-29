using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float detectionRange;
    [SerializeField] protected LayerMask targetLayer;
    protected Health m_health;
    protected Animator m_animator;

    #region 临时变量
    protected Transform m_target;

    #endregion

    #region 生命周期函数
    protected virtual void Awake()
    {
        m_health = GetComponent<Health>();
        m_animator = transform.Find("Sprite").GetComponent<Animator>();
    }
    protected virtual void OnEnable()
    {
        ComeAlive();

        m_health.OnTakeDamage -= TakeDamage;
        m_health.OnTakeDamage += TakeDamage;
        m_health.OnDeath -= Die;
        m_health.OnDeath += Die;
    }

    protected virtual void OnDisable()
    {
        m_health.OnTakeDamage -= TakeDamage;
        m_health.OnDeath -= Die;
    }

    #endregion

    #region 游戏逻辑

    protected virtual void ComeAlive()
    {
        m_health.ResetHealth();
    }
    /// <summary>
    /// 播放受伤动画，生成受伤效果
    /// </summary>
    protected virtual void TakeDamage()
    {
        
    }
    /// <summary>
    /// 死亡处理基础逻辑
    /// </summary>
    protected virtual void Die()
    {
        
    }
    /// <summary>
    /// 检测tag为Player的目标
    /// </summary>
    public virtual void DetectTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRange,
            targetLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                m_target=hit.transform;
                return;
            }
        }
    }
    

    /// <summary>
    /// 玩家进入攻击范围时，怪物需要选择的攻击策略
    /// </summary>
    public virtual void PlayerInRange()
    {
        //需要进入攻击状态（未完成）
    }
    /// <summary>
    /// 有些怪物可能存在特殊的战斗策略（如半血后狂暴之类的），在这个函数中根据传入字符串参数来选择不同的策略
    /// </summary>
    /// <param name="strategy">策略名称，需要与Unity编辑器资产中传入的customConditionType同步</param>
    public virtual void CustomStrategy(string strategy) { }

    // 初始化怪物特定逻辑
    public virtual void Initialize() { }

    // 死亡时的额外处理（如播放特效）
    protected virtual void OnDeath() { }

    // 受到伤害时的特殊反应
    public virtual void OnDamageTaken(float damage) { }
    
    #endregion

    
}
