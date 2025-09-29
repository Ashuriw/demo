using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerSpecialState
{
    public PlayerHurtState(PlayerController player) : base(player) {}

    public override int Priority => 3;

    private Coroutine hurtCoroutine;

    public override void Enter()
    {
        hurtCoroutine=player.StartCoroutine(player.GetHurtRoutine());
        //动画播放及判定逻辑（未完成）
    }
    public override void Exit()
    {
        if (hurtCoroutine!=null)
            player.StopCoroutine(hurtCoroutine);
        if(player.CurrentHealth<=0)
            player.Die();
    }
}
