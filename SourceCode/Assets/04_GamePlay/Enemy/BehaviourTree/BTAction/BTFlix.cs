using UnityEngine;
using XNode;

[NodeTint(0.7f, 0.4f, 0.9f)]
[CreateNodeMenu("Actions/Flix")]
public class BTFlix : BTNodeBase
{
    private SpriteRenderer spriteRenderer;
    protected override void OnStart()
    {
        spriteRenderer = GetRunnerComponent<EnemyActor>().SpriteRenderer;
    }
    
    public override NodeStatus OnExecute()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        Blackboard.Set("FaceDirection",spriteRenderer.flipX?-1:1);
        return NodeStatus.Success;
    }
}