using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : EnemyBase
{
    [SerializeField]protected GameObject HealthSystem;
    protected BehaviourTreeRunner runner;

    protected override void Awake()
    {
        base.Awake();
        runner = GetComponent<BehaviourTreeRunner>();
    }
    public virtual void StartBossFight()
    {
        HealthSystem.SetActive(true);
    }
    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DeathRoutine());
    }
    protected virtual IEnumerator DeathRoutine()
    {
        m_animator.SetTrigger("Death");
        
        yield return new WaitForSeconds(1f);
        
        runner.enabled = false;
        
        HealthSystem.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
