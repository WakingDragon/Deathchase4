using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BP.StarBlaster
{
    public class ShieldUI : MonoBehaviour
    {
        [SerializeField] private Slider slider = null;
        [SerializeField] private Image fill = null;
        [SerializeField] private Gradient gradient;
        float m_health;

        private void Start()
        {
            UpdateUI(1f);
        }

        public void SetHealth(float currentHealth, float maxHealth)
        {
            m_health = currentHealth / maxHealth;
            UpdateUI(m_health);
        }

        public void SetHealth(Vector2 currentAndMaxHealth)
        {
            m_health = currentAndMaxHealth.x / currentAndMaxHealth.y;
            UpdateUI(m_health);
        }

        private void UpdateUI(float health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(health);
        }
    }
}

