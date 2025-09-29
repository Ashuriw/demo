[NodeTint(0.8f, 0.4f, 0.2f)]
[CreateNodeMenu("Composites/Selector")]
public class BTSelector : BTNodeBase 
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
        
        while (currentChildIndex < children.Length) 
        {
            if (currentChild == null) 
            {
                currentChild = children[currentChildIndex];
            }
            
            var status = currentChild.Execute();
            
            if (status != NodeStatus.Failure) 
            {
                if (status != NodeStatus.Running) 
                {
                    currentChild = null;
                }
                return status;
            }
            
            currentChildIndex++;
            currentChild = null;
        }
        
        return NodeStatus.Failure;
    }
}