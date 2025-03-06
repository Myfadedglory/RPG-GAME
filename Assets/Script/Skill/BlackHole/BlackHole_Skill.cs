using UnityEngine;

namespace Script.Skill.BlackHole
{
    public class BlackholeSkill : Skill
    {
        [SerializeField] private int amountOfAttack = 4;
        [SerializeField] private float cloneAttackCoolDown = 0.3f;
        [Space]
        [SerializeField] private GameObject blackholePrefab;
        [SerializeField] private float maxSize = 15;
        [SerializeField] private float maxDuration = 20;
        [SerializeField] private float growSpeed = 1f;
        [SerializeField] private float shrinkSpeed = 3f;

        private BlackholeSkillController currentBlackhole;

        protected override void UseSkill()
        {
            base.UseSkill();

            var newBlackhole = Instantiate(blackholePrefab, Player.transform.position, Quaternion.identity);

            currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

            currentBlackhole.SetUpBlackHole(
                Player,
                maxSize,
                maxDuration, 
                growSpeed, 
                shrinkSpeed,
                amountOfAttack ,
                cloneAttackCoolDown
            );
        }

        public bool BlackholeFinished()
        {
            return currentBlackhole && currentBlackhole.PlayerCanExitState;
        }
    }
}
