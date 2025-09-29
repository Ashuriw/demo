using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEnemyConfig
{
    Rigidbody2D Rb { get; }
    Collider2D Col { get; }
    CoreStats Core {  get; }
    CombatStats Combat { get; }
    AIBehavior AI { get; }
}
