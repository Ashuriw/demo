using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimEvents : MonoBehaviour
{
    private ProjectileController m_projectile;

    void Start()
    {
        m_projectile = GetComponentInParent<ProjectileController>();
    }
    void OnTriggerDamage()
    {
        m_projectile.OnAnimationEvent_Damage();
    }

    void OnAttackEnd()
    {
        m_projectile.OnAnimationEvent_End();
    }
}
