using System.Collections;
using Script.Stats;
using UnityEngine;

namespace Script.Element
{
    public class ShockedEffect : IElementalEffect
    {
        private readonly float duration;
        private readonly Modifier shockedEffect;
        private readonly Transform currentTarget;
        private readonly float shockingRadius;

        public ShockedEffect(float duration, float attackAccurateReduceDegree, Transform currentTarget, float shockingRadius)
        {
            this.duration = duration;
            this.currentTarget = currentTarget;
            this.shockingRadius = shockingRadius;
            shockedEffect = new Modifier("Element Effect", Modifier.Operation.Multiplication, attackAccurateReduceDegree);
        }
        
        public void ApplyEffect(CharacterStats target)
        {
            target.StartCoroutine(ApplyShockedEffect(target));

            SpreadToCloseEnemies(target);
        }

        private void SpreadToCloseEnemies(CharacterStats target)
        {
            if(!target.GetComponent<Enemy.Enemy>()) return;
            
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
                if (!hit.GetComponent<Enemy.Enemy>()) continue;
                
                var distance = Vector2.Distance(detectTransform.position, hit.transform.position);

                if (distance >= closestDistance || 
                    hit.transform == detectTransform || 
                    hit.GetComponent<CharacterStats>()?.CurrentStatus == ElementalStatus.Shocked) continue;
                
                closestDistance = distance;
                
                closestEnemy = hit.transform;
            }

            return closestEnemy;
        }

        private IEnumerator ApplyShockedEffect(CharacterStats target)
        {
            target.attackAccurate.AddModifier(shockedEffect);
            yield return new WaitForSeconds(duration);
            target.attackAccurate.RemoveModifier(shockedEffect);
            target.CurrentStatus = ElementalStatus.None;
        }
    }
}