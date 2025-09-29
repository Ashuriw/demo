using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerSpecialState
{
    public PlayerJumpState(PlayerController player) : base(player){}
    
    private Coroutine m_jumpRoutine;
    
    public override int Priority =>1;

    public override void Enter()
    {
        player.m_animator.SetTrigger("Jump");
        m_jumpRoutine=player.StartCoroutine(player.JumpRoutine());
    }

    public override void Exit()
    {
        player.m_animator.SetTrigger("JumpToFall");
        player.StopCoroutine(m_jumpRoutine);
    }

    public override void Update()
    {
        if (player.DorgePressed && player.CanDorge)
            player.EnterSpecialState(player.GetState<PlayerDodgeState>());
    }
}
