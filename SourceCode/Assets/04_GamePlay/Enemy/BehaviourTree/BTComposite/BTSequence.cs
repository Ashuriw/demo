[NodeTint(0.2f, 0.4f, 0.8f)]
[CreateNodeMenu("Composites/Sequence")]
public class BTSequence : BTNodeBase 
{
    private int currentChildIndex;
    private BTNodeBase currentChild;
    
    protected override void OnStart() 
    {
        currentChildIndex = 0;
        currentChild = null;
    }
    
    public override NodeStatus OnExecute() 
    {
        var children = GetChildNodes<BTNodeBase>();
        if (children.Length == 0) return NodeStatus.Failure;
        
        if (currentChild == null) 
        {
            currentChild = children[currentChildIndex];
        }
        
        var status = currentChild.Execute();
        
        //遇到Failure立即终止
        if (status == NodeStatus.Failure) 
        {
            currentChild = null;
            return NodeStatus.Failure;
        }
        
        if (status != NodeStatus.Running) 
        {
            currentChildIndex++;
            currentChild = null;
            
            if (currentChildIndex >= children.Length) 
            {
                return status == NodeStatus.Success ? 
                    NodeStatus.Success : NodeStatus.Failure;
            }
        }
        
        return NodeStatus.Running;
    }
}