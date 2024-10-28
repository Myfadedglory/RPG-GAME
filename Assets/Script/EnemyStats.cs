using Script.Enemy.Skeleton;

namespace Script
{
    public class EnemyStats : CharacterStats
    {
        private Skeleton enemy;

        protected override void Start()
        {
            base.Start();

            enemy = GetComponent<Skeleton>();
        }

        protected override void Die()
        {
            base.Die();

            enemy.Die();
        }
    }
}
