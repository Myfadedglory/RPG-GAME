using System;
using Script.Entity.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Skill.Clone
{
    public class CloneSkillController : MonoBehaviour
    {
        private static readonly int AttackNumber = Animator.StringToHash("AttackNumber");
        private SpriteRenderer sr;
        private Animator anim;

        [SerializeField] private float colorLosingSpeed;

        [SerializeField] private Transform attackCheck; 
        [SerializeField] private float attackCheckRadius = 0.8f;

        private Vector2 cloneFacingDir = new (1,0);
        private Transform closestEnemy;
        private Func<Transform, float, Transform> findClosestEnemy;

        private CloneConfig config;
        private float cloneTimer;

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

            if(sr.color.a <= 0) Destroy(gameObject);
        }

        public void SetUpClone(
            Transform newTransform,
            CloneConfig cloneConfig,
            Func<Transform, float, Transform> findClosestEnemy,
            Vector3 offset   
        )  
        {
            transform.position = newTransform.position + offset;

            config = cloneConfig;

            this.findClosestEnemy = findClosestEnemy;
            
            cloneTimer = config.duration;

            if (config.cloneAttack.GetSkillCondition())
                anim.SetInteger(AttackNumber, Random.Range(1, 4));

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

                if (!config.duplicateClone.GetSkillCondition()) continue;
                
                if(Random.Range(0,100) < config.chanceToDuplicate * 100)
                {
                    SkillManager.instance.Clone.CreateClone(hit.transform, new Vector3(1.5f * cloneFacingDir.x, 0));
                }
            }
        }

        private void FacingToClosestTarget()
        {
            closestEnemy = findClosestEnemy(transform, config.detectDistance);

            if (!closestEnemy || closestEnemy.position.x >= transform.position.x) return;
            
            cloneFacingDir = new Vector2(-1, 0);

            transform.Rotate(0, 180, 0);
        }
    }
}
