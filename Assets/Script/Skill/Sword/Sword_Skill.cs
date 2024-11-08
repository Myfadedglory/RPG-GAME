using System;
using UnityEngine;

namespace Script.Skill.Sword
{
    public class Sword_Skill : Skill
    {
        [Header("Skill info")]
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Vector2 launchForce;
        [SerializeField] private SwordType swordType = SwordType.Regular;

        [Header("Regular Sword info")]
        [SerializeField] private float regularGravity = 3.5f;
        [SerializeField] private float rotationSwordHitDistance = 0.15f;
        [SerializeField] private float freezeDuration = 1f;

        [Header("Bounce Sword info")]
        [SerializeField] private int bounceAmount = 4;
        [SerializeField] private float bounceGravity = 3.5f;
        [SerializeField] private float maxBounceDistance = 20;
        [SerializeField] private float bounceSpeed = 20;

        [Header("Pierce Sword info")]
        [SerializeField] private int pierceAmount = 2;
        [SerializeField] private float pierceGravity = 0.1f;

        [Header("Spin Sword info")]
        [SerializeField] private float maxSpinDistance = 7f;
        [SerializeField] private float spinDuration = 1.5f;
        [SerializeField] private float spinGravity = 0.1f;
        [SerializeField] private float spinMoveSpeed = 2f;
        [SerializeField] private float hitCoolDown = 0.35f;

        [Header("Aim dots")]
        [SerializeField] private int numberOfDots;
        [SerializeField] private float spaceBetweenDots;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform dotsParent;

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
                SwordType.Bounce => bounceGravity,
                SwordType.Pierce => pierceGravity,
                SwordType.Spin => spinGravity,
                _ => regularGravity
            };
        }

        protected override void Update()
        {
            SetUpGravity();

            if (Input.GetKeyUp(KeyCode.Mouse1))
                finalDir = new Vector2(
                    AimDirection().normalized.x * launchForce.x,
                    AimDirection().normalized.y * launchForce.y
                );

            if (Input.GetKey(KeyCode.Mouse1))
            {
                for (var i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
                }
            }
        }

        public void CreateSword()
        {
            var newSword = Instantiate(swordPrefab, Player.transform.position, transform.rotation);

            var newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

            SwitchSword(newSwordScript);

            newSwordScript.SetUpSword(swordType, finalDir, swordGravity, rotationSwordHitDistance, freezeDuration, Player);

            Player.AssignNewSword(newSword);

            ActiveDots(false);
        }

        private void SwitchSword(Sword_Skill_Controller newSwordScript)
        {
            switch (swordType)
            {
                case SwordType.Bounce:
                    newSwordScript.BounceAttribute(true, bounceAmount, maxBounceDistance, bounceSpeed);
                    break;
                case SwordType.Pierce:
                    newSwordScript.PeirceAttribute(pierceAmount);
                    break;
                case SwordType.Spin:
                    newSwordScript.SpinAttribute(true, maxSpinDistance, spinDuration, hitCoolDown, spinMoveSpeed);
                    break;
                case SwordType.Regular:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Aim

        private Vector2 AimDirection()
        {
            Vector2 playerPosition = Player.transform.position;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var direction = mousePosition - playerPosition;

            return direction;
        }

        public void ActiveDots(bool _isActive)
        {
            foreach (var t in dots)
            {
                t.SetActive(_isActive);
            }
        }

        private void GenerateDots()
        {
            dots = new GameObject[numberOfDots];

            for (var i = 0; i < numberOfDots; i++)
            {
                dots[i] = Instantiate(dotPrefab, Player.transform.position, Quaternion.identity, dotsParent);
                dots[i].SetActive(false);
            }
        }

        private Vector2 DotsPosition(float t)
        {
            var position = (Vector2)Player.transform.position + new Vector2(
                AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

            return position;
        }
        #endregion
    }
}
