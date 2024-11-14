using System;
using System.Collections.Generic;
using System.Linq;
using Script.UI;
using UnityEngine;

namespace Script.Item
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        
        public List<InventoryItem> inventory;
        public Dictionary<ItemData, InventoryItem> InventoryDictionary;
        
        public List<InventoryItem> stash;
        public Dictionary<ItemData, InventoryItem> StashDictionary;
        
        public List<InventoryItem> equipment;
        public Dictionary<EquipmentData, InventoryItem> EquipmentDictionary;

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
            InventoryDictionary = new Dictionary<ItemData, InventoryItem>();
            inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
            
            stash = new List<InventoryItem>();
            StashDictionary = new Dictionary<ItemData, InventoryItem>();
            stashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
            
            equipment = new List<InventoryItem>();
            EquipmentDictionary = new Dictionary<EquipmentData, InventoryItem>();
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
            
            newEquipment.ApplyModifiers();
            
            var newItem = new InventoryItem(item);
            equipment.Add(newItem);
            EquipmentDictionary.Add(newEquipment, newItem);
            
            RemoveItem(item);
        }

        public void UnEquipItem(EquipmentType equipmentType)
        {
            var existingEquipment = EquipmentDictionary.Keys
                .FirstOrDefault(equip => equip.equipmentType == equipmentType);
            
            if(existingEquipment is null) return;
            
            if (!EquipmentDictionary.TryGetValue(existingEquipment, out var oldItem)) return;
            
            AddItem(existingEquipment);
            
            equipment.Remove(oldItem);
            existingEquipment.RemoveModifiers();
            EquipmentDictionary.Remove(existingEquipment);
        }

        #endregion

        private void UpdateSlotUI()
        {
            foreach (var m in equipmentItemSlot)
            {
                m.ClearSlot();
            }
            foreach (var t in inventoryItemSlot)
            {
                t.ClearSlot();
            }

            foreach (var i in stashItemSlot)
            {
                i.ClearSlot();
            }
            
            for (var i = 0; i < inventory.Count; i++)
            {
                inventoryItemSlot[i].UpdateSlot(inventory[i]);
            }

            for (var i = 0; i < stash.Count; i++)
            {
                stashItemSlot[i].UpdateSlot(stash[i]);
            }
            
            foreach (var slot in equipmentItemSlot)
            {
                foreach (var item in EquipmentDictionary.Where(item => item.Key.equipmentType == slot.slotType))
                {
                    slot.UpdateSlot(item.Value);
                }
            }
        }

        public void AddItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Equipment:
                    AddItemMethod(inventory, InventoryDictionary, item);
                    break;
                case ItemType.Material:
                    AddItemMethod(stash, StashDictionary, item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            UpdateSlotUI();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void RemoveItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Material:
                    RemoveItemMethod(stash, StashDictionary, item);
                    break;
                case ItemType.Equipment:
                    RemoveItemMethod(inventory, InventoryDictionary, item);
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

        private void RemoveItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> dictionary,
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
                
            UpdateSlotUI();
        }
    }
}