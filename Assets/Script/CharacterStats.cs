using UnityEngine;

namespace Script
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Major stats")]
        public Stat strength;
        public Stat agility;
        public Stat intelligence;
        public Stat vitality;

        [Header("Defensive stats")]
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion;    //����    


        public Stat damage;

        [SerializeField] private double currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth.GetValue();
        }

        public virtual void DoDamage(CharacterStats target)
        {
            var totalEvasion = evasion.GetValue() + agility.GetValue();

            if (Random.Range(1, 100) < totalEvasion)
            {
                return;
            }

            var totalDamage = damage.GetValue() + strength.GetValue();

            target.TakeDamage(totalDamage);
        }

        protected virtual void TakeDamage(double currentDamage)
        {
            currentHealth -= currentDamage;

            if (currentHealth <= 0)
                Die();
        }

        protected virtual void Die()
        {
        }
    }
}
