namespace Script.Enemy
{
    public class EnemyStats : CharacterStats
    {
        private Skeleton.Skeleton enemy;

        protected override void Start()
        {
            base.Start();

            enemy = GetComponent<Skeleton.Skeleton>();
        }

        protected override void Die()
        {
            base.Die();

            enemy.Die();
        }
    }
}
