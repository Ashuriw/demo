using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类用于桥接怪物预制体的逻辑层与表现层
/// 方便在逻辑层的控制脚本中直接访问表现层的Animator、SpriteRenderer等组件
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private Transform visual;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    public Animator Animator => m_animator ? m_animator : m_animator = visual.GetComponent<Animator>();
    public SpriteRenderer SpriteRenderer => m_spriteRenderer ?m_spriteRenderer :m_spriteRenderer = visual.GetComponent<SpriteRenderer>();
    
    // 事件定义
    public event System.Action OnAttackFrame;
    public event System.Action OnAttackEnd;

    // 供子对象调用的通知方法
    public void NotifyAttackFrame()
    {
        OnAttackFrame?.Invoke();
    }
    public void NotifyAttackEnd()
    {
        OnAttackEnd?.Invoke();
    }

    // 统一访问点示例
    public void PlayAnimation(string triggerName) 
    {
        Animator.SetTrigger(triggerName);
    }
    public void PlayAnimation(string stateName,int animState)
    {
        Animator.SetInteger(stateName, animState);
    }
}
