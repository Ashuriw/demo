[NodeTint(0.1f, 0.1f, 0.1f)]
[NodeWidth(150)]
[CreateNodeMenu("Root")]
public class BTRootNode : BTNodeBase 
{
    [Output] public NodeStatus start;
    
    public override NodeStatus OnExecute() 
    {
        var port = GetOutputPort("start");
        if (port.IsConnected) 
        {
            var child = port.Connection.node as BTNodeBase;
            return child?.Execute() ?? NodeStatus.Failure;
        }
        return NodeStatus.Failure;
    }
}