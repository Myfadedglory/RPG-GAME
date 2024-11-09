using Script.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ItemSlotUI : MonoBehaviour
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
    }
}
