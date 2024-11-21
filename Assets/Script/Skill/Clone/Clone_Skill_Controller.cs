using System;
using Script.Entity.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Skill.Clone
{
    public class Clone_Skill_Controller : MonoBehaviour
    {
        private static readonly int AttackNumber = Animator.StringToHash("AttackNumber");
        private SpriteRenderer sr;
        private Animator anim;

        [SerializeField] private float colorLosingSpeed;

        private float clonerDetectDistance;
        private float cloneTimer;

        [SerializeField] private Transform attackCheck; 
        [SerializeField] private float attackCheckRadius = 0.8f;

        private int cloneFacingDir = 1;
        private Transform closestEnemy;
        private Func<Transform, float, Transform> findClosestEnemy;

        private bool canDuplicateClone;
        private float chanceToDuplicate;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {    
            cloneTimer -= Time.deltaTime;

            if (cloneTimer >= 0) return;
            
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if(sr.color.a <= 0)
                Destroy(gameObject);
        }

        public void SetUpClone(
            Transform newTransform,
            float cloneDuration,
            float clonerDetectDistance,
            bool canAttack,
            Func<Transform, float, Transform> FindClosestEnemy,
            bool canDuplicateClone,
            float chanceToDuplicate,
            Vector3 _offset   
        )  
        {
            transform.position = newTransform.position + _offset;

            cloneTimer = cloneDuration;

            this.findClosestEnemy = FindClosestEnemy;

            this.canDuplicateClone = canDuplicateClone;

            this.chanceToDuplicate = chanceToDuplicate;

            if (canAttack)
                anim.SetInteger(AttackNumber, Random.Range(1, 4));

            this.clonerDetectDistance = clonerDetectDistance;

            FacingToClosestTarget();
        }


        private void AnimationTrigger()
        {
            cloneTimer = -.1f;
        }

        private void AttackTrigger()
        {
            var colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Entity.Enemy.Enemy>() == null) continue;
                
                hit.GetComponent<Entity.Enemy.Enemy>().Damage(PlayerManager.instance.player.Stats,cloneFacingDir);

                if (!canDuplicateClone) continue;
                
                if(Random.Range(0,100) < chanceToDuplicate * 100)
                {
                    SkillManager.instance.Clone.CreateClone(hit.transform, new Vector3(1.5f * cloneFacingDir, 0));
                }
            }
        }

        private void FacingToClosestTarget()
        {
            closestEnemy = findClosestEnemy(transform, clonerDetectDistance);

            if (!closestEnemy || closestEnemy.position.x >= transform.position.x) return;
            
            cloneFacingDir = -1;

            transform.Rotate(0, 180, 0);
        }
    }
}
