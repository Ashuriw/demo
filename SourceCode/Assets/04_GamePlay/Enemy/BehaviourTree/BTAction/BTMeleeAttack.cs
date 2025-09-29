using UnityEngine;

[NodeTint(0.7f, 0.2f, 0.2f)]
[CreateNodeMenu("Actions/Melee Attack")]
public class BTMeleeAttack : BTAttackBase 
{
    public int damage = 20;
    public float range = 2f;
    public string targetKey = "CurrentTarget";
    [SerializeField] private GameObject hitBoxPrefab;
    
    protected override void OnStart()
    {
        base.OnStart();
        actor.OnAttackFrame += TriggerDamage;
    }

    protected override void OnStop()
    {
        base.OnStop();
        actor.OnAttackFrame-=TriggerDamage;
    }

    protected override IAttackLogic CreateAttackLogic() 
    {
        return new MeleeAttackLogic(
            damage, 
            range, 
            Blackboard.Get<GameObject>(targetKey)
        );
    }

    private void TriggerDamage()
    {
        int faceDirection=Blackboard.Get<int>("FaceDirection");
        GameObject hitBox=Instantiate(hitBoxPrefab, actor.transform.position, actor.transform.rotation);
        HitBox hitBoxComponent = hitBox.GetComponent<HitBox>();
        hitBoxComponent.ActivateHitBox(faceDirection);
    }
}
