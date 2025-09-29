using UnityEngine;

public class PlayerParryState : PlayerBasicState
{
    public PlayerParryState(PlayerController player) : base(player){}

    public override void Enter()
    {
        player.m_animator.SetInteger("AnimState",3);
        player.Parry = true;
    }
    public override void Exit()
    {
        player.Parry = false;
    }

    public override void Update()
    {
        if(!player.ParryPressed)
            player.ChangeState(player.GetState<PlayerIdleState>());
        if(!player.IsGrounded)
            player.ChangeState(player.GetState<PlayerFallState>());
        if(player.MovePressed)
            player.ChangeState(player.GetState<PlayerRunState>());
    }
}
