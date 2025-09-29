using UnityEngine;

[NodeTint(0.2f, 0.8f, 0.6f)]
[CreateNodeMenu("Conditions/Check Distance")]
public class BTCheckDistance : BTNodeBase 
{
    public string targetKey = "CurrentTarget";
    public float maxDistance = 10f;

    public override NodeStatus OnExecute() 
    {
        var target = Blackboard.Get<GameObject>(targetKey);
        var runner = GetRunner();
        if (target == null || runner == null) return NodeStatus.Failure;

        float distanceSqr = Vector3.SqrMagnitude(runner.transform.position-target.transform.position);
        return (distanceSqr<maxDistance*maxDistance)? NodeStatus.Success : NodeStatus.Failure;
    }
}