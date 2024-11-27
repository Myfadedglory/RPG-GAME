using System;

namespace Script.Item.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData data;
        public int stackSize;

        public InventoryItem(ItemData data)
        {
            this.data = data;
            AddStack();
        }

        public void AddStack(int amount = 1)
        {
            stackSize+=amount;
        }

        public void RemoveStack(int amount = 1)
        {
            stackSize-=amount;
        }
    }
}