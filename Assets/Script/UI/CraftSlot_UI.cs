using Script.Item;
using Script.Item.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class CraftSlotUI : MonoBehaviour, IPointerClickHandler
    {
        public ItemData itemData;
        private UI ui;
        
        private void Start()
        {
            ui = GetComponentInParent<UI>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemData == null) return;
            
            if(ui.craftTooltip == null) return;
            
            ui.craftTooltip.HideTooltip();
            
            ui.craftTooltip.ShowTooltip(itemData as EquipmentData);
        }

        public void CopyFrom(ItemData itemData)
        {
            this.itemData = itemData;
            var image = GetComponent<Image>();
            image.sprite = itemData.icon;
        }
    }
}