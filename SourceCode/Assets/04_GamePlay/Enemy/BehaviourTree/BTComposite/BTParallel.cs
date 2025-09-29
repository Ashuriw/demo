[NodeTint(0.4f, 0.2f, 0.8f)]
[CreateNodeMenu("Composites/Parallel")]
public class BTParallel : BTNodeBase 
{
    public Policy successPolicy = Policy.RequireAll;
    public Policy failurePolicy = Policy.RequireOne;
    
    private BTNodeBase[] children;
    private NodeStatus[] statuses;
    
    public enum Policy { RequireOne, RequireAll }
    
    protected override void OnStart() 
    {
        children = GetChildNodes<BTNodeBase>();
        statuses = new NodeStatus[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            statuses[i] = NodeStatus.Inactive;
        }
    }
    
    public override NodeStatus OnExecute() 
    {
        bool hasSuccess = false;
        bool hasFailure = false;
        bool allFinished = true;
        
        for (int i = 0; i < children.Length; i++) 
        {
            if (statuses[i] == NodeStatus.Inactive || 
                statuses[i] == NodeStatus.Running) 
            {
                statuses[i] = children[i].Execute();
                allFinished = false;
            }
            
            if (statuses[i] == NodeStatus.Success) hasSuccess = true;
            if (statuses[i] == NodeStatus.Failure) hasFailure = true;
        }
        
        if (allFinished) 
        {
            if (hasFailure && failurePolicy == Policy.RequireOne) 
                return NodeStatus.Failure;
            if (hasSuccess && successPolicy == Policy.RequireOne) 
                return NodeStatus.Success;
            return NodeStatus.Failure;
        }
        
        return NodeStatus.Running;
    }
}