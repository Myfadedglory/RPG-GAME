using System;
using System.Collections.Generic;
using System.Text;
using Script.Entity.Player;
using Script.Item.Inventory;
using Script.Stats;
using UnityEngine;

namespace Script.Item.Equipment
{
    [Serializable]
    [CreateAssetMenu(fileName = "new Item Menu", menuName = "Data/Equipment")]
    public class EquipmentData : ItemData
    {
        public EquipmentType equipmentType;
        
        private StringBuilder materialDescription = new ();

        public bool locked = false;

        public string unlockedCondition;
        
        public List<Modifier> modifiers = new ();

        public List<InventoryItem> craftMaterials = new ();

        public void ApplyModifiers()
        {
            foreach (var modifier in modifiers)
            {
                PlayerManager.instance.player.GetComponent<PlayerStats>().ApplyModifier(modifier);
            }
        }

        public void RemoveModifiers()
        {
            foreach (var modifier in modifiers)
            {
                PlayerManager.instance.player.GetComponent<PlayerStats>().RemoveModifier(modifier);
            }
        }

        public override void ClearExtraMessage()
        {
            base.ClearExtraMessage();
            materialDescription.Clear();
        }

        public override string GetAttributeDescription()
        {
            foreach (var modifier in modifiers)
            {
                AddItemDescription(modifier.GetValue(), modifier.GetStatType().ToString());
            }

            return Description.ToString();
        }

        public string GetRawMaterialDescription()
        {
            foreach (var item in craftMaterials)
            {
                AddMaterialDescription(item.data.itemName,item.stackSize);
            }
            
            return materialDescription.ToString();
        }
        
        private void AddItemDescription(double value, string name)
        {
            if (Description.Length > 0) Description.AppendLine();

            if (value > 0) Description.Append(name + ": " + value);
        }

        private void AddMaterialDescription(string materialName, int materialValue)
        {
            if (materialDescription.Length > 0) materialDescription.AppendLine();
            
            if (materialValue > 0) materialDescription.Append(materialName + ": " + materialValue);
        }

        public void Unlock()
        {
            locked = false;
        }
    }
}