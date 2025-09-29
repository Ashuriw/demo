using Unity.IO.LowLevel.Unsafe;

public class PlayerHitState : PlayerSpecialState
{
    public PlayerHitState(PlayerController player) : base(player) { }
    
    public override int Priority => 3;

    public override void Enter()
    {
        player.m_animator.SetTrigger("Hit");
        player.Invincible = true;
    }

    public override void Exit()
    {
        player.Invincible = false;
    }
}
