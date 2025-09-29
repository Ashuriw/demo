using UnityEngine;

public enum TriggerType
{
    OnCollision,    // 碰撞触发
    OnAnimationEvent // 动画事件触发
}

public class ProjectileController : MonoBehaviour
{
    [Header("移动设置")]
    public float speed = 10f;
    public float arrivalDistance = 0.5f;
    public Vector3 moveDirection;
    public bool useTargetPosition;
    public Vector3 targetPosition;
    
    [Header("触发设置")]
    public TriggerType triggerType;
    public float lifeTime = 3f;
    public GameObject hitEffect;
    
    [Header("伤害设置")]
    public GameObject hitBoxPrefab;
    
    private float timer;
    private Animator animator;
    private bool isActive = true;
    private bool hasReachedDestination;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public void Initialize(Vector3 direction, Vector3 targetPos = default)
    {
        moveDirection = direction.normalized;
        if (useTargetPosition) targetPosition = targetPos;
        
        timer = 0f;
        isActive = true;
        hasReachedDestination = false;
        
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        timer += Time.deltaTime;
        
        // 移动逻辑
        if (useTargetPosition && !hasReachedDestination)
        {
            Vector3 toTarget = targetPosition - transform.position;
            float distance = toTarget.magnitude;
            
            if (distance <= arrivalDistance)
            {
                OnReachDestination();
            }
            else
            {
                transform.position += toTarget.normalized * speed * Time.deltaTime;
            }
        }
        else if (!useTargetPosition)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        
        // 生命周期检查
        if (timer >= lifeTime)
        {
            Deactivate();
        }
    }
    
    private void OnReachDestination()
    {
        hasReachedDestination = true;
        transform.position = targetPosition; // 确保精确到达
        
        if (animator != null)
        {
            animator.SetTrigger("Execute"); // 播放到达后的激活动画
        }
    }
    
    // 由动画事件调用
    public void OnAnimationEvent_Damage()
    {
        if (triggerType == TriggerType.OnAnimationEvent)
        {
            TriggerDamage();
        }
    }
    
    private void TriggerDamage()
    {
        if (hitBoxPrefab != null)
        {
            GameObject hitBox = Instantiate(hitBoxPrefab, transform.position, Quaternion.identity);
            HitBox hitBoxComponent = hitBox.GetComponent<HitBox>();
            hitBoxComponent.ActivateHitBox(1);
        }
    }
    private void ColliderDamage()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
    
    // 由动画事件调用
    public void OnAnimationEvent_End()
    {
        Deactivate();
    }
    
    private void Deactivate()
    {
        isActive = false;
        ReturnToPool();
    }
    
    private void ReturnToPool()
    {
        ProjectilePool.Instance.ReturnToPool(ProjectilePool.Instance.pools[0].prefab, gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || triggerType != TriggerType.OnCollision) return;
        
        if (other.CompareTag("Player"))
        {
            ColliderDamage();
            Deactivate();
        }
    }
}