namespace Script.Player
{
    public class PlayerStats : CharacterStats
    {
        private Script.Player.Player player;

        protected override void Start()
        {
            base.Start();

            player = GetComponent<Script.Player.Player>();
        }

        protected override void Die()
        {
            base.Die();

            player.Die();
        }
    }
}
