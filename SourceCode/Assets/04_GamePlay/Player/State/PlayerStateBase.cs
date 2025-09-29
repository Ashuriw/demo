using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase
{
    protected PlayerController player;
    public abstract bool IsSpecialState { get; }

    protected PlayerStateBase(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }

    public virtual void FixedUpdate() { }
}
