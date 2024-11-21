using UnityEngine;

namespace Script.Item
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        
        private Rigidbody2D Rb => GetComponent<Rigidbody2D>();
        private SpriteRenderer Sr => GetComponent<SpriteRenderer>();

        private void OnValidate()
        {
            if (itemData == null) return;
            
            Sr.sprite = itemData.icon;
            
            gameObject.name = "Item Object - " + itemData.itemName;
        }

        public void SetUp(ItemData itemData, Vector2 velocity)
        {
            this.itemData = itemData;
            Rb.velocity = velocity;
            Sr.sprite = itemData.icon;
            gameObject.name = itemData.itemName;
        }

        public void PickUpItem()
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
