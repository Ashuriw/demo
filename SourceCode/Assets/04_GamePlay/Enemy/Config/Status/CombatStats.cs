// 战斗属性模块
using UnityEngine;

[System.Serializable]
public struct CombatStats
{
    [Header("战斗属性参数")]
    public float attackRange;
    public float attackRate;
    public int damage;
    public float knockbackResistance;
}