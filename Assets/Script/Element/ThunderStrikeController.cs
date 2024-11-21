using Script.Entity.Player;
using Script.Stats;
using UnityEngine;

namespace Script.Element
{
    public class ThunderStrikeController : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("Hit");
        
        private CharacterStats target;
        private float speed;
        private double damage;
        
        private Animator anim;
        private bool trigger;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        public void SetUp(CharacterStats target, float speed, double damage)
        {
            this.target = target;
            this.damage = damage;
            this.speed = speed;
        }

        private void Update()
        {
            if(trigger || !target) return;
            
            transform.position = Vector2.MoveTowards(
                transform.position, 
                target.transform.position, 
                speed * Time.deltaTime
            );
            transform.right = transform.position - target.transform.position;
            
            if (Vector2.Distance(transform.position, target.transform.position) > 0.1f) return;

            anim.transform.localPosition = new Vector3(0, 0.5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            
            trigger = true;
            DamageAndSelfDestroy();
            anim.SetTrigger(Hit);
            
        }

        private void DamageAndSelfDestroy()
        {
            var enemy = target.GetComponent<Entity.Enemy.Enemy>();
            
            enemy.MagicDamage(PlayerManager.instance.player.Stats, MagicType.Lightning);

            Destroy(gameObject, 0.4f);
        }
    }
}
