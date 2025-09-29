using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Blackboard {
    // 基础类型存储
    private Dictionary<string, object> data = new Dictionary<string, object>();
    
    // 专用字段（避免字符串key的拼写错误）
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private Vector3 lastKnownPosition;
    
    // 动态存取方法
    public T Get<T>(string key) => data.ContainsKey(key) ? (T)data[key] : default;
    public void Set<T>(string key, T value) => data[key] = value;
    
    // 专用属性封装
    public GameObject CurrentTarget {
        get => currentTarget;
        set {
            currentTarget = value;
            if (value != null) lastKnownPosition = value.transform.position;
        }
    }
    
    public Vector3 LastKnownPosition => lastKnownPosition;
    
    // 时间相关
    public float LastAttackTime { get; private set; }
    public void RecordAttackTime() => LastAttackTime = Time.time;
}