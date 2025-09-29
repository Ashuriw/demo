using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
	public Image currentHealthBar;
	public Image currentHealthGlobe;
	public Text healthText;
	public Health m_health;

	// public Image currentManaBar;
	// public Image currentManaGlobe;

	private void Awake()
	{
		if (m_health == null)
			m_health = GameObject.FindWithTag("Player").GetComponent<Health>();
	}
	private void OnEnable()
	{
		m_health.OnHealthChanged -= UpdateGraphics;
		m_health.OnHealthChanged += UpdateGraphics;
	}

	private void OnDisable()
	{
		m_health.OnHealthChanged -= UpdateGraphics;
	}
	void Start()
	{
		UpdateGraphics();
	}

	
	private void UpdateHealthBar()
	{
		if (currentHealthBar == null) return;
		float ratio = (float)m_health.currentHealth / m_health.maxHealth;
		currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
		if(healthText != null)
			healthText.text = m_health.currentHealth.ToString("0") + "/" + m_health.maxHealth.ToString("0");
	}

	private void UpdateHealthGlobe()
	{
		if (currentHealthGlobe == null) return;
		float ratio = (float)m_health.currentHealth / m_health.maxHealth;
		currentHealthGlobe.rectTransform.localPosition = new Vector3(0, currentHealthGlobe.rectTransform.rect.height * ratio - currentHealthGlobe.rectTransform.rect.height, 0);
		if(healthText != null)
			healthText.text = m_health.currentHealth.ToString("0") + "/" + m_health.maxHealth.ToString("0");
	}


	private void UpdateGraphics()
	{
		UpdateHealthBar();
		UpdateHealthGlobe();
		// UpdateManaBar();
		// UpdateManaGlobe();
	}

	
}
