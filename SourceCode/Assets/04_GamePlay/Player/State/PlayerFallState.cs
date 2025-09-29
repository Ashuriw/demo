using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBasicState
{
    public PlayerFallState(PlayerController player) : base(player) {}

    public override void Enter()
    {
        //进入坠落状态时执行的逻辑
        player.m_animator.SetInteger("AnimState",2);
    }
   
    public override void Update()
    {
        base.Update();

        if (player.IsGrounded)
        {
                player.ChangeState(player.GetState<PlayerIdleState>());
        }
    }
}
