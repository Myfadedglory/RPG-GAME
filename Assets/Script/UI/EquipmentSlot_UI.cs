using Script.Item;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class EquipmentSlotUI : ItemSlotUI
    {
        public EquipmentType slotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment slot " + slotType;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Inventory.instance.UnEquipItem(slotType);
        }
    }
}
