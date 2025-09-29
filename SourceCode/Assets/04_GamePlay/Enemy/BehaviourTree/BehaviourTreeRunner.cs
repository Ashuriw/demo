using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour 
{
    public BehaviourTree behaviourTree;
    
    void Start() 
    {
        if (behaviourTree != null) 
        {
            behaviourTree.rootNode = FindRootNode();
            behaviourTree.Blackboard.Set("FaceDirection", 1);
            behaviourTree.Blackboard.Set("TurningKey", false);
            behaviourTree.Blackboard.CurrentTarget = null;
        }
    }
    
    void Update() 
    {
        behaviourTree?.Execute(gameObject);
    }
    
    private BTRootNode FindRootNode() 
    {
        foreach (var node in behaviourTree.nodes) 
        {
            if (node is BTRootNode root) 
            {
                return root;
            }
        }
        return null;
    }
}