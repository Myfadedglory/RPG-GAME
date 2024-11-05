using System.Collections;
using UnityEngine;

namespace Script
{
    public class IgniteEffect : IElementalEffect
    {
        private readonly float duration;
        private readonly double burningDamage;
        private readonly float damageInterval;

        public IgniteEffect(float duration)
        {
            this.duration = duration;
        }
        
        public void ApplyEffect(CharacterStats target)
        {
            target.StartCoroutine(BurnDamageCoroutine(target));
        }
        
        private IEnumerator BurnDamageCoroutine(CharacterStats target)
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