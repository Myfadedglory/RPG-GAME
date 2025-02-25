using System.Text;
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
        public string description;
        
        protected readonly StringBuilder Description = new ();

        protected virtual string GetBasicDescription()
        {
            return description;
        }

        public virtual void ClearExtraMessage()
        {
            Description.Clear();
        }

        public virtual string GetAttributeDescription()
        {
            return " ";
        }
    }
}