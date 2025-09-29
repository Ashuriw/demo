using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBasicState : PlayerStateBase
{
    protected PlayerBasicState(PlayerController player) : base(player){}
    
    public override bool IsSpecialState => false;

    public override void Update()
    {
        CheckBattleState();
    }
    private void CheckBattleState()
    {
        if (player.DorgePressed && player.CanDorge)
            player.EnterSpecialState(player.GetState<PlayerDodgeState>());
        if (player.AttackPressed)
            player.EnterSpecialState(player.GetState<PlayerAttackState>());
    }

    public override void FixedUpdate()
    {
        player.CheckIsGrounded();
        player.HandleSpriteRenderer();
        player.HandleMovement();
        player.HandleJump();
    }
}
