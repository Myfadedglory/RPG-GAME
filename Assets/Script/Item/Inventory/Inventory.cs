using System;
using System.Collections.Generic;
using System.Linq;
using Script.Item.Equipment;
using Script.UI;
using UnityEngine;

namespace Script.Item.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        
        public List<InventoryItem> inventory;
        private Dictionary<ItemData, InventoryItem> inventoryDictionary;
        
        public List<InventoryItem> stash;
        private Dictionary<ItemData, InventoryItem> stashDictionary;
        
        public List<InventoryItem> equipment;
        private Dictionary<EquipmentData, InventoryItem> equipmentDictionary;

        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform stashSlotParent;
        [SerializeField] private Transform equipmentSlotParent;
        
        private ItemSlotUI[] inventoryItemSlot;
        private ItemSlotUI[] stashItemSlot;
        private EquipmentSlotUI[] equipmentItemSlot;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            inventory = new List<InventoryItem>();
            inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
            inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
            
            stash = new List<InventoryItem>();
            stashDictionary = new Dictionary<ItemData, InventoryItem>();
            stashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
            
            equipment = new List<InventoryItem>();
            equipmentDictionary = new Dictionary<EquipmentData, InventoryItem>();
            equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
        }

        #region Equip Logic

        public void EquipItem(ItemData item)
        {
            if (item is not EquipmentData newEquipment)
            {
                Debug.LogError("Item is not of type EquipmentData");
                return;
            }
            
            UnEquipItem(newEquipment.equipmentType);
            RemoveItem(item);
            
            newEquipment.ApplyModifiers();
            
            var newItem = new InventoryItem(item);
            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
            
            /*
            *   updateSlot这里必须在Add方法后面，不然更新会延时生效,也就是必须要在更新Dictionary后再更新Slot
            */
            UpdateSlotUI(equipmentItemSlot);
        }

        public void UnEquipItem(EquipmentType equipmentType)
        {
            var existingEquipment = equipmentDictionary.Keys
                .FirstOrDefault(equip => equip.equipmentType == equipmentType);
            
            if(existingEquipment is null) return;
            
            if (!equipmentDictionary.TryGetValue(existingEquipment, out var oldItem)) return;
            
            AddItem(existingEquipment);
            
            equipment.Remove(oldItem);
            existingEquipment.RemoveModifiers();
            equipmentDictionary.Remove(existingEquipment);
            
            /*
             *   updateSlot这里必须在Add方法后面，不然更新会延时生效,也就是必须要在更新Dictionary后再更新Slot
             */
            UpdateSlotUI(equipmentItemSlot);
        }

        #endregion

        #region Updqte Slot
        
        private static void UpdateSlotUI(List<InventoryItem> items, ItemSlotUI[] slots)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (i < items.Count)
                {
                    slots[i].UpdateSlot(items[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }

        private void UpdateSlotUI(EquipmentSlotUI[] slots)
        {
            foreach (var slot in slots)
            {
                slot.ClearSlot();
                foreach (var item in equipmentDictionary.Where(item => item.Key.equipmentType == slot.slotType))
                {
                    slot.UpdateSlot(item.Value);
                }
            }
        }
        
        #endregion

        #region Add or Remove Api and Method
        
        public void AddItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Equipment:
                    AddItemMethod(inventory, inventoryDictionary, item);
                    UpdateSlotUI(inventory, inventoryItemSlot);
                    break;
                case ItemType.Material:
                    AddItemMethod(stash, stashDictionary, item);
                    UpdateSlotUI(stash, stashItemSlot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void RemoveItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Material:
                    RemoveItemMethod(stash, stashDictionary, item);
                    UpdateSlotUI(stash, stashItemSlot);
                    break;
                case ItemType.Equipment:
                    RemoveItemMethod(inventory, inventoryDictionary, item);
                    UpdateSlotUI(inventory, inventoryItemSlot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AddItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> dictionary,
            ItemData item)
        {
            if (dictionary.TryGetValue(item, out var value))
            {
                value.AddStack();
            }
            else
            {
                var newItem = new InventoryItem(item);
                items.Add(newItem);
                dictionary.Add(item, newItem);
            }
        }

        private static void RemoveItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> dictionary,
            ItemData item)
        {
            if (!dictionary.TryGetValue(item, out var value)) return;
            
            if (value.stackSize <= 1)
            {
                items.Remove(value);
                dictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
        #endregion

        public bool CanCraft(EquipmentData itemToCraft, List<InventoryItem> requiredMaterials)
        {
            var materialToRemove = new List<InventoryItem>();
            
            foreach (var material in requiredMaterials)
            {
                if (stashDictionary.TryGetValue(material.data, out var stashValue))
                {
                    if (stashValue.stackSize < material.stackSize)
                    {
                        Debug.Log("Don't have enough material");
                        return false;
                    }
                    else
                    {
                        materialToRemove.Add(stashValue);
                    }
                }
                else
                {
                    Debug.Log("Don't have enough material");
                    return false;
                }
            }

            foreach (var removedMaterial in materialToRemove)
            {
                RemoveItem(removedMaterial.data);
            }
            
            AddItem(itemToCraft);
            return true;
        }
    }
}