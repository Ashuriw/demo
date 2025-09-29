using JetBrains.Annotations;
using XNode;
using UnityEngine;

public enum NodeStatus { Success, Failure, Running, Inactive }

public abstract class BTNodeBase : Node 
{
    [Input(connectionType = ConnectionType.Override)] public NodeStatus enter;
    [Output(connectionType = ConnectionType.Multiple)] public NodeStatus exit;
    
    [HideInInspector] public NodeStatus lastStatus;
    protected Blackboard Blackboard => (graph as BehaviourTree)?.Blackboard;


    protected override void Init()
    {
        base.Init();
        lastStatus = NodeStatus.Inactive;
    }
    
    protected virtual void OnStart() {}
    protected virtual void OnStop() {}
    
    public virtual NodeStatus OnExecute() 
    {
        return NodeStatus.Success;
    }
    
    public NodeStatus Execute() 
    {
        if (lastStatus == NodeStatus.Inactive) 
        {
            OnStart();
        }
        
        lastStatus = OnExecute();
        
        if (lastStatus != NodeStatus.Running) 
        {
            OnStop();
            var result=lastStatus;
            lastStatus = NodeStatus.Inactive;
            return result;
        }
        
        return lastStatus;
    }
    
    public T GetChildNode<T>() where T : BTNodeBase 
    {
        var port = GetOutputPort("exit");
        if (port != null && port.IsConnected) 
        {
            return port.Connection.node as T;
        }
        return null;
    }
    public T[] GetChildNodes<T>() where T : BTNodeBase 
    {
        var port = GetOutputPort("exit");
        if (port != null && port.IsConnected) 
        {
            // 获取所有连接的节点
            var connections = port.GetConnections();
            var nodes = new System.Collections.Generic.List<T>();
            foreach (var connection in connections) 
            {
                if (connection.node is T node) 
                {
                    nodes.Add(node);
                }
            }
            return nodes.ToArray();
        }
        return new T[0];
    }
    /// <summary>
    /// 安全获取执行器组件的方法
    /// </summary>
    /// <typeparam name="T">执行器上的组件</typeparam>
    /// <returns></returns>
    protected T GetRunnerComponent<T>() where T:Component
    {
        var runner = Blackboard?.Get<GameObject>("Executor");
        return runner != null ? runner.GetComponent<T>() : null;
    }
    protected GameObject GetRunner()
    {
        return Blackboard?.Get<GameObject>("Executor");
    }
}