using Script.Item;
using Script.Item.Equipment;
using Script.Item.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.UI
{
    public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;
        [SerializeField] private GameObject throwPrefab;

        private UI ui;
        public InventoryItem item;

        private void Start()
        {
            ui = GetComponentInParent<UI>();
        }

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
            if(item == null) return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.instance.RemoveItem(item.data);
                ThrowItem(item.data);
            }
                
            
            if(item.data.itemType == ItemType.Equipment)
                Inventory.instance.EquipItem(item.data);
        }

        private void ThrowItem(ItemData item)
        {
            var pos = transform.position + new Vector3(1,1);
            var newThrow = Instantiate(throwPrefab, pos, Quaternion.identity);
            if (!newThrow.TryGetComponent(out ItemObject itemObject)) return;
            var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 15));
            itemObject.SetUp(item, randomVelocity);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(item == null) return;
            
            if(item.data == null || item.data.itemType != ItemType.Equipment) return;
            
            ui.tooltip.ShowTooltip(item.data as EquipmentData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(item == null) return;
            
            if(item.data == null || item.data.itemType != ItemType.Equipment) return;
            
            ui.tooltip.HideTooltip();
        }
    }
}
