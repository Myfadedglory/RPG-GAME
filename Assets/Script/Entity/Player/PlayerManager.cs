using UnityEngine;

namespace Script.Entity.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;
        public Player player;

        private void Awake()
        {
            if (instance != null)
                Destroy(instance);
            else
                instance = this;
        }
    }
}
