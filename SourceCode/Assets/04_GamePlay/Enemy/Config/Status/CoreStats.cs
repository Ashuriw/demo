// 基础属性模块
using UnityEngine;

[System.Serializable]
public struct CoreStats
{
    [Header("基础属性参数")]
    public string displayName;
    public int maxHealth;
    public float moveSpeed;
    public float detectionRange;
    public LayerMask targetLayer;
    public float deathDelay;
}
