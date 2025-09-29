using UnityEngine;

[NodeTint(0.8f, 0.2f, 0.6f)]
[CreateNodeMenu("Conditions/Check Health")]
public class BTCheckHealth : BTNodeBase 
{
    public float threshold = 0.3f;
    public bool aboveThreshold = true;
    
    public override NodeStatus OnExecute() 
    {
        // 获取血量组件
        var health = GetRunnerComponent<Health>();
        if (health == null) return NodeStatus.Failure;
        
        float healthPercent = health.currentHealth / health.maxHealth;
        bool conditionMet = aboveThreshold ? 
            healthPercent > threshold : 
            healthPercent <= threshold;
        
        return conditionMet ? 
            NodeStatus.Success : NodeStatus.Failure;
    }
}