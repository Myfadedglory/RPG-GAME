using UnityEngine;

namespace Script.Item
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        private void OnValidate()
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            
            gameObject.name = "Item Object - " + itemData.itemName;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Inventory.instance.AddItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}
