using Script.Entity.Player;
using Script.Stats;
using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class StatSlotUI : MonoBehaviour
    {
        [SerializeField] private string statName;
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private TextMeshProUGUI statNameText;

        private void OnValidate()
        {
            gameObject.name = "Stat - " + statName;

            if (statValueText != null)
            {
                statNameText.text = statName;
            }
        }

        private void Start()
        {
            UpdateStatValueUI();
        }

        private void Update()
        {
            var playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            
            if (!playerStats) return;

            playerStats.GetStat(statType).StatValueChanged += UpdateStatValueUI;
        }

        private void UpdateStatValueUI()
        {
            var playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            
            if (!playerStats) return;
            
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }
}