// AI行为模块
using UnityEngine;

[System.Serializable]
public struct AIBehavior
{
    [Header("AI行为参数")]
    public float patrolRadius;
    public float idleDuration;
    public float chaseSpeedMultiplier;
}