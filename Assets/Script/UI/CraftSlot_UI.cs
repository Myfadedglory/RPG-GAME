using System;
using Script.Item;
using Script.Item.Equipment;
using Script.Item.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class CraftSlotUI : ItemSlotUI
    {
        public void OnEnable()
        {
            UpdateSlot(item);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            var craftData = item.data as EquipmentData;
            if (craftData != null && Inventory.instance.CanCraft(craftData, craftData.craftMaterials))
            {
                Debug.Log("you create a craft");
            }
        }
    }
}