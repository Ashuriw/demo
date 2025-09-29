[NodeTint(0.8f, 0.2f, 0.2f)]
[CreateNodeMenu("Decorators/Inverter")]
public class BTInverter : BTNodeBase 
{
    public override NodeStatus OnExecute() 
    {
        var child = GetChildNode<BTNodeBase>();
        if (child == null) return NodeStatus.Failure;
        
        var status = child.Execute();
        
        return status switch 
        {
            NodeStatus.Success => NodeStatus.Failure,
            NodeStatus.Failure => NodeStatus.Success,
            _ => status
        };
    }
}