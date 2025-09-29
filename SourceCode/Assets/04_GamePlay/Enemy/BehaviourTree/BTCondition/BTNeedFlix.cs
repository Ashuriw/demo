using UnityEngine;

[NodeTint(0.7f, 0.5f, 0.8f)]
[CreateNodeMenu("Conditions/Need Flix")]
public class BTNeedFlix : BTNodeBase
{
    [SerializeField] private float _turnThreshold = 0.5f; // 转向阈值
        
    private Transform _bossTransform;
    private Transform _playerTransform;
    
    protected override void OnStart() 
    {
        _bossTransform = GetRunner().transform;
        _playerTransform = Blackboard.Get<GameObject>("CurrentTarget").transform;
    }
    public override NodeStatus OnExecute()
    {
        if (Blackboard.Get<bool>("LockTurning"))
            return NodeStatus.Failure;
        Vector3 toPlayer = _playerTransform.position - _bossTransform.position;
        float dot = Vector3.Dot(toPlayer.normalized, -1*Blackboard.Get<int>("FaceDirection")*_bossTransform.right);
        
        // 阈值判断（避免微小位移导致抖动）
        if (Mathf.Abs(dot) < _turnThreshold) 
        {
            return NodeStatus.Failure; // 无需转向
        }
        
        return dot < 0 ? NodeStatus.Success : NodeStatus.Failure;
    }
}
