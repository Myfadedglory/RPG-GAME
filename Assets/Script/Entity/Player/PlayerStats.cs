using Script.Stats;

namespace Script.Entity.Player
{
    public class PlayerStats : CharacterStats
    {
        private Player player;

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player>();
        }

        protected override void Die()
        {
            base.Die();

            player.Die();
        }
    }
}
