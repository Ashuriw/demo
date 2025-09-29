using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerSpecialState
{
    private Coroutine dodgeCoroutine;

    public PlayerDodgeState(PlayerController player) : base(player) {}
    
    public override int Priority => 2;

    public override void Enter()
    {
        dodgeCoroutine=player.StartCoroutine(player.DorgeRoutine());
        //动画播放及判定逻辑
        player.m_animator.SetTrigger("Dodge");
        player.Invincible = true;
    }
    public override void Exit()
    {
        if (dodgeCoroutine!=null)
            player.StopCoroutine(dodgeCoroutine);
        player.StopMovement();
        player.Invincible = false;
    }

    public override void OnAnimationEnd()
    {
        player.ExitSpecialState();
    }
}
