using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Item
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private GameObject dropPrefab;
        [SerializeField] private List<ItemDropAndChance> possibleDrop;
        [SerializeField] private List<ItemData> droppedItems = new ();

        public virtual void GenerateDrop()
        {
            foreach (var drop in possibleDrop.Where(drop => Random.Range(0, 100) <= drop.chance))
            {
                droppedItems.Add(drop.itemData);
                var number = Random.Range(drop.minDropNumber, drop.maxDropNumber);
                for (var i = 0; i < number; i++)
                {
                    DropItem(drop.itemData);
                }
            }
        }

        protected void DropItem(ItemData item)
        {
            var pos = transform.position + new Vector3(0, 1);
            var newDrop = Instantiate(dropPrefab, pos, Quaternion.identity);
            if (!newDrop.TryGetComponent(out ItemObject itemObject)) return;
            var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 15));
            itemObject.SetUp(item, randomVelocity);
        }
    }
}