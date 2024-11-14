using System.Collections;
using UnityEngine;

namespace Script.Element.effect
{
    public class IgniteEffect : ElementalEffectBase
    {
        private readonly double burningDamage;
        private readonly float damageInterval;

        public IgniteEffect(float duration, double burningDamage, float damageInterval) 
            : base(duration)
        {
            this.burningDamage = burningDamage;
            this.damageInterval = damageInterval;
        }

        protected override IEnumerator EffectCoroutine()
        {
            var timer = Duration;

            while (timer > 0)
            {
                Target.TakeDamage(burningDamage);
                yield return new WaitForSeconds(damageInterval);
                timer -= damageInterval;
            }

            Target.CurrentStatus = ElementalStatus.None;
        }
    }
}