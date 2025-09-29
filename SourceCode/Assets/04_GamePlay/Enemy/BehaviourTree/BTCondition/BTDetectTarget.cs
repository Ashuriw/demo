using XNode;
using UnityEngine;

[NodeTint(0.6f, 0.7f, 0.4f)]
[CreateNodeMenu("Conditions/BTDetectTarget")]
public class BTDetectTarget : BTNodeBase
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool drawGizmos = true;
    private bool m_isActive = false;

    protected override void OnStop()
    {
        m_isActive = false;
    }
    
    public override NodeStatus OnExecute()
    {
        // 如果黑板已有目标，直接返回成功
        if (Blackboard.CurrentTarget != null)
        {
            return NodeStatus.Success;
        }
        m_isActive = true;
        
        // 执行玩家检测
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            GetRunner().transform.position,
            detectionRadius,
            playerLayer);
            
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Blackboard.CurrentTarget = hit.gameObject;
                Blackboard.Set("CurrentTarget", hit.gameObject);
                BossBase boss = GetRunnerComponent<BossBase>();
                if (boss != null)boss.StartBossFight();
                return NodeStatus.Success;
            }
        }
        
        return NodeStatus.Failure;
    }
    

}
