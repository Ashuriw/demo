[NodeTint(0.6f, 0.2f, 0.8f)]
[CreateNodeMenu("Decorators/Until Fail")]
public class BTUntilFail : BTNodeBase 
{
    public override NodeStatus OnExecute() 
    {
        var child = GetChildNode<BTNodeBase>();
        if (child == null) return NodeStatus.Failure;
        
        var status = child.Execute();
        return status == NodeStatus.Failure ? 
            NodeStatus.Success : NodeStatus.Running;
    }
}