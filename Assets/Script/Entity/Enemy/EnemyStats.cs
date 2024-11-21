using Script.Item;
using Script.Stats;

namespace Script.Entity.Enemy
{
    public class EnemyStats : CharacterStats
    {
        private Skeleton.Skeleton enemy;
        private ItemDrop dropSystem;

        protected override void Start()
        {
            base.Start();

            enemy = GetComponent<Skeleton.Skeleton>();
            
            dropSystem = GetComponent<ItemDrop>();
        }

        protected override void Die()
        {
            base.Die();

            enemy.Die();
            
            dropSystem.GenerateDrop();
        }
    }
}
