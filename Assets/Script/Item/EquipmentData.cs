using System;
using System.Collections.Generic;
using Script.Player;
using Script.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Item
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "new Item Menu", menuName = "Data/Equipment")]
    public class EquipmentData : ItemData
    {
        public EquipmentType equipmentType;
        
        public List<Modifier> modifiers = new ();

        public void ApplyModifiers()
        {
            var playerStats = PlayerManger.instance.player.GetComponent<PlayerStats>();
            
            foreach (var modifier in modifiers)
            {
                playerStats.ApplyModifier(modifier);
            }
        }

        public void RemoveModifiers()
        {
            var playerStats = PlayerManger.instance.player.GetComponent<PlayerStats>();

            foreach (var modifier in modifiers)
            {
                playerStats.RemoveModifier(modifier);
            }
        }
    }
}