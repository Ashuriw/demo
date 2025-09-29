using UnityEngine;
using XNode;

[NodeTint(0.4f, 0.7f, 0.9f)]
[CreateNodeMenu("Actions/Remote Attack")]
public class BTRemoteAttack : BTAttackBase
{
    [Header("投射物设置")]
    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private Vector3[] spawnOffsets;
    [SerializeField] private Vector3[] targetOffsets;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float range = 8f;
    [SerializeField] private string targetKey = "CurrentTarget";
    
    [Header("触发设置")]
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private float arrivalDistance = 0.5f;
    
    [Header("伤害设置")]
    [SerializeField] private GameObject hitBoxPrefab;
    
    protected override IAttackLogic CreateAttackLogic()
    {
        // 预配置所有投射物
        foreach (var projectile in projectilePrefabs)
        {
            var controller = projectile.GetComponent<ProjectileController>();
            if (controller != null)
            {
                controller.triggerType = triggerType;
                controller.arrivalDistance = arrivalDistance;
                controller.hitBoxPrefab = hitBoxPrefab;
            }
        }
        
        return new RemoteAttackLogic(
            projectilePrefabs,
            spawnOffsets,
            targetOffsets,
            projectileSpeed,
            Blackboard.Get<GameObject>(targetKey),
            range,
            GetRunner().transform,
            triggerType);
    }
}