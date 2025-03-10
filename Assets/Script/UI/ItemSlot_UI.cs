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
            
            // 先处理 itemImage 的默认状态
            if (itemImage != null)
            {
                itemImage.color = (item != null) ? Color.white : Color.clear;
                itemImage.sprite = item?.data.icon;
            }

            if (itemText != null)
            {
                itemText.text = item is { stackSize: > 1 } ? item.stackSize.ToString() : "";
            }
        }

        public void ClearSlot()
        {
            item = null;
            if (itemImage != null)
            {
                itemImage.sprite = null;
                itemImage.color = Color.clear;
            }
           
            if (itemText != null)
            {
                itemText.text = "";
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(item == null) return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.instance.RemoveItem(item.data);
                ThrowItem(item.data);
            }

            if (item.data == null) return;
            
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
            
            Vector2 mousePosition = Input.mousePosition;
            
            var xOffset = 75;
            var yOffset = 25;

            if (mousePosition.x > Screen.width * 0.5f)
            {
                xOffset *= -1;
            }

            if (mousePosition.y > Screen.height * 0.5f)
            {
                yOffset *= -1;
            }
            
            if(item.data == null || item.data.itemType != ItemType.Equipment) return;
            
            ui.tooltip.ShowTooltip(item.data as EquipmentData);
            ui.tooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(item == null) return;
            
            if(item.data == null || item.data.itemType != ItemType.Equipment) return;
            
            ui.tooltip.HideTooltip();
        }
    }
}
