using UnityEngine;

public class BringerofDeathAnimEvents : MonoBehaviour
{
    private BringerofDeath m_bringerofDeath;
    private EnemyActor m_actor;
    private AudioManager m_audioManager;

    void Start()
    {
        m_bringerofDeath = GetComponentInParent<BringerofDeath>();
        m_actor = GetComponentInParent<EnemyActor>();
        m_audioManager = AudioManager.instance;
    }

    // 动画事件方法
    public void OnAnimEvent_AttackFrame()
    {
        m_actor?.NotifyAttackFrame();
        
        m_audioManager.PlaySound("BossAttack");
    }
    public void OnAnimEvent_AttackEnd()
    {
        m_actor?.NotifyAttackEnd();
    }

    public void OnSkill1()
    {
        m_bringerofDeath.OnSkill1();
    }
}
