using XNode;
using UnityEngine;

[CreateAssetMenu(fileName = "New Behavior Tree", menuName = "Assets/04_GamePlay/Enemy/BehaviourTree/BehaviourTree.cs")]
public class BehaviourTree : NodeGraph 
{
    [HideInInspector] public BTRootNode rootNode;
    public Blackboard Blackboard{get; private set;}=new  Blackboard();
    
    public void Execute(GameObject runner) 
    {
        Blackboard.Set("Executor", runner);
        if (rootNode != null) 
        {
            rootNode.Execute();
        }
    }
    
    public NodeStatus GetStatus() 
    {
        return rootNode?.lastStatus ?? NodeStatus.Failure;
    }
}