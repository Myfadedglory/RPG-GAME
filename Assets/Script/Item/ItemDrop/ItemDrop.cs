using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Item.ItemDrop
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private GameObject dropPrefab;
        [SerializeField] private List<ItemDropAndChance> possibleDrop;
        [SerializeField] private List<ItemData> droppedItems = new ();

        public virtual void GenerateDrop()
        {
            // 1. 收集所有符合条件的掉落项
            var selectedDrops = possibleDrop.Where(drop => Random.Range(0, 100) <= drop.chance).ToList();

            // 2. 如果没有掉落项且列表不为空，强制添加一个（默认选第一个）
            if (selectedDrops.Count == 0 && possibleDrop.Count > 0)
                selectedDrops.Add(possibleDrop[0]);

            // 3. 处理最终选中的掉落项
            foreach (var drop in selectedDrops)
            {
                droppedItems.Add(drop.itemData);
                var number = Random.Range(drop.minDropNumber, drop.maxDropNumber);
                for (var i = 0; i < number; i++)
                {
                    DropItem(drop.itemData);
                }
            }
        }

        protected virtual void DropItem(ItemData item)
        {
            var pos = transform.position + new Vector3(0, 1);
            var newDrop = Instantiate(dropPrefab, pos, Quaternion.identity);
            if (!newDrop.TryGetComponent(out ItemObject itemObject)) return;
            var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 15));
            itemObject.SetUp(item, randomVelocity);
        }
    }
}