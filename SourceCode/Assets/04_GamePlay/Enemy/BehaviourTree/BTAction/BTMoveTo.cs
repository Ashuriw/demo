using UnityEngine;

[NodeTint(0.4f, 0.8f, 0.2f)]
[CreateNodeMenu("Actions/Move To")]
public class BTMoveTo : BTNodeBase
{
    public string targetKey = "CurrentTarget";
    public string stateName = "AnimState";
    public float stoppingDistance = 0.5f;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private EnemyActor actor;

    protected override void OnStart() {
        rb = GetRunnerComponent<Rigidbody2D>();
        actor = GetRunnerComponent<EnemyActor>();
        actor.PlayAnimation(stateName,1);
    }

    protected override void OnStop()
    {
        actor.PlayAnimation(stateName,0);
    }

    public override NodeStatus OnExecute() {
        var target = Blackboard.Get<GameObject>(targetKey);
        if (target == null || rb == null) return NodeStatus.Failure;

        Vector2 toTarget = (Vector2)target.transform.position - rb.position;
        toTarget.y = 0;

        if (toTarget.sqrMagnitude <= stoppingDistance * stoppingDistance) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return NodeStatus.Success;
        }

        rb.velocity = toTarget.normalized * moveSpeed;
        return NodeStatus.Running;
    }
}