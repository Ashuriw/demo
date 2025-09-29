[NodeTint(0.8f, 0.6f, 0.2f)]
[CreateNodeMenu("Decorators/Repeater")]
public class BTRepeater : BTNodeBase 
{
    public int repeatCount = 3;
    private int currentCount;
    
    protected override void OnStart() 
    {
        currentCount = 0;
    }
    
    public override NodeStatus OnExecute() 
    {
        var child = GetChildNode<BTNodeBase>();
        if (child == null) return NodeStatus.Failure;
        
        var status = child.Execute();
        
        if (status != NodeStatus.Running) 
        {
            currentCount++;
            if (currentCount >= repeatCount) 
            {
                return NodeStatus.Success;
            }
        }
        
        return NodeStatus.Running;
    }
}