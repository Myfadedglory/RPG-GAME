using System.Collections.Generic;
using Script.UI;
using UnityEngine;

namespace Script.Item
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        public List<InventoryItem> inventoryItems;
        public Dictionary<ItemData, InventoryItem> InventoryDictionary;

        [SerializeField] private Transform inventorySlotParent;
        
        private ItemSlotUI[] itemSlot;
        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            inventoryItems = new List<InventoryItem>();
            
            InventoryDictionary = new Dictionary<ItemData, InventoryItem>();

            itemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        }

        private void UpdateSlotUI()
        {
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                itemSlot[i].UpdateSlot(inventoryItems[i]);
            }
        }

        public void AddItem(ItemData item)
        {
            if (InventoryDictionary.TryGetValue(item, out var value))
            {
                value.AddStack();
            }
            else
            {
                var newItem = new InventoryItem(item);
                inventoryItems.Add(newItem);
                InventoryDictionary.Add(item,newItem);
            }
            
            UpdateSlotUI();
        }

        public void RemoveItem(ItemData item)
        {
            if (!InventoryDictionary.TryGetValue(item, out var value)) return;
            
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                InventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
            
            UpdateSlotUI();
        }
    }
}