using System;
using System.Collections.Generic;
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

        public override string GetAttributeDescription()
        {
            foreach (var modifier in modifiers)
            {
                AddItemDescription(modifier.GetValue(), modifier.GetStatType().ToString());
            }

            return Sb.ToString();
        }
        
        private void AddItemDescription(double value, string name)
        {
            if (Sb.Length > 0) Sb.AppendLine();

            if (value > 0) Sb.Append(name + ": " + value);
        }
    }
}