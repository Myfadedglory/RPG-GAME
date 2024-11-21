using System.Collections;
using Script.Stats;
using UnityEngine;

namespace Script.Element.effect
{
    public class ShockedEffect : ElementalEffectBase
    {
        private readonly Modifier shockedEffect;
        private readonly Transform currentTarget;
        private readonly float shockingRadius;

        public ShockedEffect(float duration, float attackAccurateReduceDegree, Transform currentTarget, float shockingRadius) 
            : base(duration)
        {
            this.currentTarget = currentTarget;
            this.shockingRadius = shockingRadius;
            shockedEffect = new Modifier("Element Effect", StatType.LightningMagic, Modifier.Operation.Multiplication, attackAccurateReduceDegree);
        }

        protected override IEnumerator EffectCoroutine()
        {
            Target.ApplyModifier(shockedEffect);

            SpreadToCloseEnemies(Target);

            yield return new WaitForSeconds(Duration);

            Target.attackAccurate.RemoveModifier(shockedEffect);
            Target.CurrentStatus = ElementalStatus.None;
        }

        private void SpreadToCloseEnemies(CharacterStats target)
        {
            if (!target.GetComponent<Entity.Enemy.Enemy>()) return;

            var newTarget = FindClosestEnemy(currentTarget, shockingRadius)?.GetComponent<CharacterStats>();

            if (newTarget is not null && newTarget.CurrentStatus != ElementalStatus.Shocked)
            {
                currentTarget.GetComponent<CharacterStats>().CreateShockStrike(newTarget);
            }
        }

        private static Transform FindClosestEnemy(Transform detectTransform, float radius)
        {
            var colliders = Physics2D.OverlapCircleAll(detectTransform.position, radius);

            Transform closestEnemy = null;
            var closestDistance = Mathf.Infinity;

            foreach (var hit in colliders)
            {
                if (!hit.GetComponent<Entity.Enemy.Enemy>()) continue;

                var distance = Vector2.Distance(detectTransform.position, hit.transform.position);

                if (distance >= closestDistance || 
                    hit.transform == detectTransform || 
                    hit.GetComponent<CharacterStats>()?.CurrentStatus == ElementalStatus.Shocked) continue;

                closestDistance = distance;
                closestEnemy = hit.transform;
            }

            return closestEnemy;
        }
    }
}
