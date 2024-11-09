using UnityEngine;

namespace Script.Item
{
    public enum ItemType
    {
        Material,
        Equipment
    }
    
    [CreateAssetMenu(fileName = "new Item Menu", menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public ItemType itemType;
        public string itemName;
        public Sprite icon;
    }
}