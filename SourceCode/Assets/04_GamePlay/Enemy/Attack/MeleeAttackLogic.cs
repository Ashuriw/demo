using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackLogic : IAttackLogic 
{
    private readonly int damage;
    private readonly float range;
    private readonly GameObject target;

    public MeleeAttackLogic(int damage, float range, GameObject target) 
    {
        this.damage = damage;
        this.range = range;
        this.target = target;
    }

    public bool CanExecute(GameObject executor) 
    {
        if (target == null) return false;
        return Vector3.SqrMagnitude(executor.transform.position-target.transform.position) <= range*range;
    }

    public void ExecuteAttack(GameObject executor) 
    {
        
    }
}
