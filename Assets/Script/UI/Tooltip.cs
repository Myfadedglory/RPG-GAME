using Script.Item.Equipment;
using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;

        public void ShowTooltip(EquipmentData item)
        {
            if (item == null) return;
            itemNameText.text = item.itemName;
            itemTypeText.text = item.equipmentType.ToString();
            itemDescriptionText.text = item.GetAttributeDescription();

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}