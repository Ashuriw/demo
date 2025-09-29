using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAttackLogic : IAttackLogic
{
    private readonly GameObject[] projectiles;
    private readonly Vector3[] spawnOffsets;
    private readonly Vector3[] targetOffsets;
    private readonly int damage;
    private readonly float speed;
    private readonly GameObject target;
    private readonly float range;
    private readonly Transform executorTransform;
    private readonly TriggerType triggerType;
    
    public RemoteAttackLogic(
        GameObject[] projectiles,
        Vector3[] spawnOffsets,
        Vector3[] targetOffsets,
        float speed,
        GameObject target,
        float range,
        Transform executorTransform,
        TriggerType triggerType)
    {
        this.projectiles = projectiles;
        this.spawnOffsets = spawnOffsets;
        this.targetOffsets = targetOffsets;
        this.speed = speed;
        this.target = target;
        this.range = range;
        this.executorTransform = executorTransform;
        this.triggerType = triggerType;
    }
    
    public bool CanExecute(GameObject executor)
    {
        if (target == null) return false;
        return Vector3.SqrMagnitude(executor.transform.position - target.transform.position) <= range * range;
    }
    
    public void ExecuteAttack(GameObject executor)
    {
        Vector3 baseTargetPosition = target != null ? target.transform.position : executor.transform.position + executor.transform.forward * 5f;
        
        for (int i = 0; i < projectiles.Length; i++)
        {
            Vector3 spawnPos = executorTransform.position + spawnOffsets[i % spawnOffsets.Length];
            Vector3 targetPos = baseTargetPosition + targetOffsets[i % targetOffsets.Length];
            Vector3 direction = (targetPos - spawnPos).normalized;
            
            GameObject projectile = ProjectilePool.Instance.GetFromPool(
                projectiles[i % projectiles.Length],
                spawnPos,
                Quaternion.identity);
            
            if (projectile != null)
            {
                ProjectileController controller = projectile.GetComponent<ProjectileController>();
                if (controller != null)
                {
                    controller.triggerType = triggerType;
                    controller.speed = speed;
                    controller.useTargetPosition = triggerType == TriggerType.OnAnimationEvent;
                    controller.Initialize(direction, targetPos);
                }
            }
        }
    }
}
