using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerSpecialState
{
    public PlayerAttackState(PlayerController player) : base(player) {}
    
    public override int Priority => 0;

    private const int m_comboCount = 3;
    private Coroutine m_comboRoutine;
    private int m_comboCounter=0;
    private bool m_nextComboInputChecked=false;
    private bool m_isComboTransition=false;

    public override void OnAnimationEnd()
    {
        if (m_nextComboInputChecked)
        {
            NextCombo();
        }
        else
        {
            m_comboRoutine=player.StartCoroutine(ComboRoutine());
        }
    }
    public override void Enter()
    {
        //动画播放及判定逻辑
        player.StopMovement();
        player.m_animator.SetInteger("Combo", ++m_comboCounter);
        player.m_animator.SetTrigger("Attack");
    }
    public override void Exit()
    {
        if(m_comboRoutine!=null)
            player.StopCoroutine(m_comboRoutine);
        ResetCombo();
        m_nextComboInputChecked = false;
        m_isComboTransition = false;
    }
    public override void Update()
    {
        
        if (player.DorgePressed && player.CanDorge)
            player.EnterSpecialState(player.GetState<PlayerDodgeState>());
        if(!m_nextComboInputChecked&&player.AttackPressed)
            m_nextComboInputChecked = true;
    }

    public override void FixedUpdate()
    {
        player.CheckIsGrounded();
        player.HandleJump();
        player.HandleSpriteRenderer();
    }

    private void NextCombo()
    {
        m_isComboTransition = true;
        player.EnterSpecialState(player.GetState<PlayerAttackState>());
    }

    private void ResetCombo()
    {
        if (m_comboCounter >= m_comboCount||!m_isComboTransition)
        {
            m_comboCounter = 0;
            player.m_animator.SetInteger("Combo", 0);
        }
    }
    
    private IEnumerator ComboRoutine()
    {
        float comboTimer = 0;
        while (comboTimer < player.ComboTimer)
        {
            comboTimer+= Time.deltaTime;
            if (m_nextComboInputChecked)
            {
                NextCombo();
                yield break;
            }
            if (player.ParryPressed||player.MovePressed||!player.IsGrounded)
            {
                player.ExitSpecialState();
                yield break;
            }
            yield return null;
        }
        player.ExitSpecialState();
    }
}
