using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSpecialState : PlayerStateBase
{
    protected PlayerSpecialState(PlayerController player):base(player) {}
    
    public override bool IsSpecialState => true;
    
    public abstract int Priority { get; }
    
    public virtual void OnAnimationEnd(){}
}
