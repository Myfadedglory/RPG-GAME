namespace Script
{
    public class PlayerStats : CharacterStats
    {
        private Player.Player player;

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Player.Player>();
        }

        protected override void Die()
        {
            base.Die();

            player.Die();
        }
    }
}
