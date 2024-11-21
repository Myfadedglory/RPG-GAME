using Script.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;

        public InventoryItem item;

        public void UpdateSlot(InventoryItem item)
        {
            this.item = item;
            
            itemImage.color = Color.white;
            
            if (item == null) return;

            itemImage.sprite = item.data.icon;

            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }

        public void ClearSlot()
        {
            item = null;
            
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemText.text = "";
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(Input.GetKey(KeyCode.LeftControl))
                Inventory.instance.RemoveItem(item.data);
            
            if(item.data.itemType == ItemType.Equipment)
                Inventory.instance.EquipItem(item.data);
        }
    }
}
