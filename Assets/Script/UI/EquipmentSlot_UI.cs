using Script.Item;

namespace Script.UI
{
    public class EquipmentSlotUI : ItemSlotUI
    {
        public EquipmentType slotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment slot " + slotType;
        }
    }
}
