using UnityEngine;

[NodeTint(0.6f, 0.6f, 0.6f)]
[CreateNodeMenu("Actions/Wait")]
public class BTWait : BTNodeBase 
{
    public float waitTime = 2f;
    private float startTime;
    
    protected override void OnStart() 
    {
        startTime = Time.time;
    }
    
    public override NodeStatus OnExecute() 
    {
        if (Time.time - startTime >= waitTime) 
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Running;
    }
}