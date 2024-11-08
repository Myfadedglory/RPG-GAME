using Script.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        private Entity entity;
        private RectTransform healthBar;
        private Slider healthSlider;
        private CharacterStats characterStats;

        private void Awake()
        {
            entity = GetComponentInParent<Entity>();
            healthBar = GetComponent<RectTransform>();
            healthSlider = GetComponentInChildren<Slider>();
            characterStats = GetComponentInParent<CharacterStats>();
        }

        private void UpdateHealthBar()
        {
            healthSlider.maxValue = (float) characterStats.GetMaxHealth();
            healthSlider.value = (float) characterStats.currentHealth;
        }

        private void FlipUI() => healthBar.Rotate(0f, 180f, 0f);

        private void OnEnable()
        {
            entity.OnFlipped += FlipUI;
            characterStats.OnHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            entity.OnFlipped -= FlipUI;
            characterStats.OnHealthChanged -= UpdateHealthBar;
        }
    }
}