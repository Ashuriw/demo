using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerIdleState : PlayerBasicState
{
    public PlayerIdleState(PlayerController player) : base(player) {}

    public override void Enter()
    {
        //进入闲置状态时执行的逻辑
         player.m_animator.SetInteger("AnimState", 0);
    }

    
    public override void Update()
    {
        base.Update();

        if (!player.IsGrounded)
            player.ChangeState(player.GetState<PlayerFallState>());
        else if (player.MovePressed)
            player.ChangeState(player.GetState<PlayerRunState>());
        else if(player.ParryPressed)
            player.ChangeState(player.GetState<PlayerParryState>());
    }
}
