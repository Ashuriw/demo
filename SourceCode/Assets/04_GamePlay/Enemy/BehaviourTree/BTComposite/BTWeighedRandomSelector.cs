using UnityEngine;
using System;
using System.Linq;

[NodeTint(0.3f, 0.1f, 0.6f)]
[CreateNodeMenu("Composites/Weighted Random")]
public class BTWeightedRandomSelector : BTNodeBase 
{
    [Serializable]
    public struct WeightedNode 
    {
        public BTNodeBase node;
        [Range(0, 10)] public int weight;
    }

    [SerializeField] private WeightedNode[] _weightedChildren;
    private BTNodeBase _currentChild;

    protected override void OnStart() 
    {
        _currentChild = SelectWeightedChild();
    }

    public override NodeStatus OnExecute() 
    {
        if (_currentChild == null) return NodeStatus.Failure;

        var status = _currentChild.Execute();

        if (status != NodeStatus.Running) 
        {
            _currentChild = SelectWeightedChild();
        }

        return status;
    }

    private BTNodeBase SelectWeightedChild() 
    {
        int totalWeight = _weightedChildren.Sum(c => c.weight);
        int random = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var child in _weightedChildren) 
        {
            currentWeight += child.weight;
            if (random < currentWeight) 
            {
                return child.node;
            }
        }

        return null;
    }
}