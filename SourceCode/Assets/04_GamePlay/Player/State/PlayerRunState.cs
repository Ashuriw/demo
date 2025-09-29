using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerRunState : PlayerBasicState
{
    public PlayerRunState(PlayerController player):base(player) {}

    public override void Enter()
    {
        //进入跑步状态时执行的逻辑
        player.m_animator.SetInteger("AnimState", 1);
    }
    
    public override void Update()
    {
        base.Update();

        if (!player.IsGrounded)
            player.ChangeState(player.GetState<PlayerFallState>());
        else if (!player.MovePressed)
            player.ChangeState(player.GetState<PlayerIdleState>());
        else if(player.ParryPressed)
            player.ChangeState(player.GetState<PlayerParryState>());
    }
}
