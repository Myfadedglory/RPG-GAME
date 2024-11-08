using System.Collections;
using Script.Stats;
using UnityEngine;

namespace Script.Element
{
    public class IgniteEffect : IElementalEffect
    {
        private readonly float duration;
        private readonly double burningDamage;
        private readonly float damageInterval;

        public IgniteEffect(float duration, double burningDamage, float damageInterval)
        {
            this.duration = duration;
            this.burningDamage = burningDamage;
            this.damageInterval = damageInterval;
        }
        
        public void ApplyEffect(CharacterStats target)
        {
            target.StartCoroutine(ApplyIgniteEffect(target));
        }
        
        private IEnumerator ApplyIgniteEffect(CharacterStats target)
        {
            var timer = duration;
            
            while (timer > 0)
            {
                target.TakeDamage(burningDamage);
                
                yield return new WaitForSeconds(damageInterval);
                
                timer -= damageInterval;
            }

            if (timer < 0)
            {
                target.CurrentStatus = ElementalStatus.None;
            }
        }
    }
}