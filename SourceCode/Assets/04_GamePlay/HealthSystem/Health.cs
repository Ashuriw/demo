using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float invincibleTime=0.5f;
    public event System.Action OnTakeDamage;
    public event System.Action OnHealthChanged;
    public event System.Action OnDeath;
    public bool Invincible
    {
        get { return m_invincible; }
        set { m_invincible = value; }
    }

    private bool m_invincible=false;
    private float m_invincibleTimer=0.0f;
    private bool m_parry = false;

    public void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke();
        if (!m_parry)
        {
            currentHealth -= damage;
            OnHealthChanged?.Invoke();
        }
        else
        {
            m_parry = false;
        }
        if(currentHealth <= 0)
            Die();
        StartCoroutine(InvincibleCoroutine());
    }
    private void Die()
    {
        OnDeath?.Invoke();
    }
    public void ResetHealth()
    {
        OnHealthChanged?.Invoke();
        currentHealth = maxHealth;
    }

    public void Parry()
    {
        m_parry = true;
    }

    private IEnumerator InvincibleCoroutine()
    {
        m_invincibleTimer = 0;
        m_invincible = true;
        while (m_invincibleTimer < invincibleTime)
        {
            m_invincibleTimer += Time.deltaTime;
            yield return null;
        }
        m_invincible = false;
    }
}
