using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfig",menuName = "Assets/04_GamePlay/Player/Config/PlayerConfig.cs")]
public class PlayerConfig : ScriptableObject
{
    [Header("移动参数")]
    public float moveSpeed = 8f;
    public float acceleration = 15f;
    public float airControl = 0.5f;
    public float deceleration = 30f;

    [Header("跳跃参数")]
    public float jumpForce = 16f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float jumpVelocityToFall = 1f;
    public float jumpDuration = 0.2f;

    [Header("战斗参数")]
    public int maxHealth = 100;
    public float invincibleDuration = 0.5f;
    public float dorgeCoolDown = 1f;
    public float dorgeForce = 16f;
    public float knockBackForce = 5f;
    public float detectionRadius = 10f;
    public LayerMask targetLayer;
    public float comboTimer = 1.5f;

    [Header("动画参数")]
    public float attackDuration = 0.1f;
    public float dorgeDuration = 0.1f;

    [Header("系统设置")]
    public LayerMask groundLayer;
    public float widthRatio = 0.8f;
    public float checkDistance = 0.1f;
    public float checkInterval = 0.1f;
}
