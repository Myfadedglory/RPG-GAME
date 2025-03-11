using UnityEngine;

namespace Script.Skill.Sword
{
    public class SwordSkill : Skill
    {
        [SerializeField] private SwordConfig swordConfig;
        [SerializeField] private SwordType swordType = SwordType.Regular;
        
        [Header("Skill Condition")]
        [SerializeField] private SkillCondition throwSword;
        [SerializeField] private SkillCondition throwBounceSword;
        [SerializeField] private SkillCondition throwPierceSword;
        [SerializeField] private SkillCondition throwSpinSword;

        private GameObject[] dots;
        private float swordGravity;
        private Vector2 finalDir;

        protected override void Start()
        {
            base.Start();
            GenerateDots();
            SetUpGravity();
        }

        private void SetUpGravity()
        {
            swordGravity = swordType switch
            {
                SwordType.Bounce => swordConfig.bounceGravity,
                SwordType.Pierce => swordConfig.pierceGravity,
                SwordType.Spin => swordConfig.spinGravity,
                _ => swordConfig.regularGravity
            };
        }

        protected override void Update()
        {
            base.Update();
            
            SetUpSword();

            if (Input.GetKeyUp(KeyCode.Mouse1))
                finalDir = new Vector2(
                    AimDirection().normalized.x * swordConfig.launchForce.x,
                    AimDirection().normalized.y * swordConfig.launchForce.y
                );

            if (Input.GetKey(KeyCode.Mouse1))
            {
                for (var i = 0; i < dots.Length; i++)
                    dots[i].transform.position = DotsPosition(i * swordConfig.spaceBetweenDots);
            }
        }

        private void SetUpSword()
        {
            if (throwBounceSword.GetSkillCondition())
                swordType = SwordType.Bounce;
            else if (throwPierceSword.GetSkillCondition())
                swordType = SwordType.Pierce;
            else if (throwSpinSword.GetSkillCondition())
                swordType = SwordType.Spin;
            else
                swordType = SwordType.Regular;
            
            SetUpGravity();
        }

        public void CreateSword()
        {
            var newSword = Instantiate(swordConfig.swordPrefab, Player.transform.position, transform.rotation);

            var newSwordScript = newSword.GetComponent<SwordSkillController>();

            newSwordScript.SetUpSword(swordType, finalDir, swordGravity, swordConfig, Player);

            Player.AssignNewSword(newSword);

            ActiveDots(false);
        }

        #region Aim

        private Vector2 AimDirection()
        {
            Vector2 playerPosition = Player.transform.position;

            if (!Camera.main) return Vector2.zero;
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var direction = mousePosition - playerPosition;

            return direction;
        }

        public void ActiveDots(bool isActive)
        {
            foreach (var t in dots)
            {
                t.SetActive(isActive);
            }
        }

        private void GenerateDots()
        {
            dots = new GameObject[swordConfig.numberOfDots];

            for (var i = 0; i < swordConfig.numberOfDots; i++)
            {
                dots[i] = Instantiate(swordConfig.dotPrefab, Player.transform.position, Quaternion.identity, swordConfig.dotsParent);
                dots[i].SetActive(false);
            }
        }

        private Vector2 DotsPosition(float t)
        {
            var position = (Vector2)Player.transform.position + new Vector2(
                AimDirection().normalized.x * swordConfig.launchForce.x,
                AimDirection().normalized.y * swordConfig.launchForce.y) * t + Physics2D.gravity * (0.5f * swordGravity * (t * t));

            return position;
        }
        #endregion
    }
}
