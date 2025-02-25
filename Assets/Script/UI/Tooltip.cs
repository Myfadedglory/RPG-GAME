using Script.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] protected Image image;
        [SerializeField] protected TextMeshProUGUI itemNameText;
        [SerializeField] protected TextMeshProUGUI itemTypeText;
        [SerializeField] protected TextMeshProUGUI itemDescriptionText;
        
        protected EquipmentData CurrentEquipment;

        public virtual void ShowTooltip(EquipmentData item)
        {
            if (item == null) return;
            image.sprite = item.icon;
            itemNameText.text = item.itemName;
            itemTypeText.text = item.equipmentType.ToString();
            itemDescriptionText.text = item.GetAttributeDescription();

            gameObject.SetActive(true);
            
            CurrentEquipment = item;
        }

        public virtual void HideTooltip()
        {
            gameObject.SetActive(false);
            
            if (CurrentEquipment == null) return;
            
            CurrentEquipment.ClearExtraMessage();
        }
    }
}