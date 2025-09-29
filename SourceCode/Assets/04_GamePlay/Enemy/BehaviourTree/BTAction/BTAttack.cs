using UnityEngine;
public abstract class BTAttackBase : BTNodeBase 
{
    [SerializeField] protected string animationTrigger;
    protected IAttackLogic attackLogic;
    protected EnemyActor actor;
    private bool isAttacking;
    private bool attackCompleted;

    protected override void OnStart() 
    {
        actor = GetRunnerComponent<EnemyActor>();
        attackLogic = CreateAttackLogic();
        isAttacking = false;
        Blackboard.Set("LockTurning", true);

        attackCompleted = false;
        actor.OnAttackEnd += CompleteAttack;
    }

    protected override void OnStop()
    {
        actor.OnAttackEnd -= CompleteAttack;
    }

    public override NodeStatus OnExecute() 
    {
        if (actor == null || attackLogic == null) 
            return NodeStatus.Failure;

        if (!isAttacking) 
        {
            if (attackLogic.CanExecute(actor.gameObject)) 
            {
                actor.PlayAnimation(animationTrigger);
                isAttacking = true;
                Blackboard.Set("LockTurning", false);
                return NodeStatus.Running;
            }
            return NodeStatus.Failure;
        }
        else 
        {
            // 等待动画播放完成
            if (!attackCompleted) 
                return NodeStatus.Running;
            
            attackLogic.ExecuteAttack(actor.gameObject);
            isAttacking = false;
            return NodeStatus.Success;
        }
    }

    protected abstract IAttackLogic CreateAttackLogic();

    private void CompleteAttack()
    {
        attackCompleted = true;
    }
}