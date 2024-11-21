using Script.Stats;

namespace Script.Entity.Player
{
    public class PlayerStats : CharacterStats
    {
        private Script.Entity.Player.Player player;

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Script.Entity.Player.Player>();
        }

        protected override void Die()
        {
            base.Die();

            player.Die();
        }
    }
}
