using UnityEngine;

namespace Script.Item
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }
    
    [CreateAssetMenu(fileName = "new Item Menu", menuName = "Data/Equipment")]
    public class EquipmentData : ItemData
    {
        public EquipmentType equipmentType;
    }
}