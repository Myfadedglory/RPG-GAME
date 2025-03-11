using System;
using UnityEngine;

namespace Script.Skill.Sword
{
    [Serializable]
    public class SwordConfig
    {
        [Header("Skill info")]
        public GameObject swordPrefab;
        public Vector2 launchForce;

        [Header("Regular Sword info")]
        public float regularGravity = 3.5f;
        public float rotationSwordHitDistance = 0.15f;
        public float freezeDuration = 1f;
        public float returnSpeed = 12f;
        public float catchSwordDistance = 1f;

        [Header("Bounce Sword info")]
        public int bounceAmount = 4;
        public float bounceGravity = 3.5f;
        public float maxBounceDistance = 20;
        public float bounceSpeed = 20;

        [Header("Pierce Sword info")]
        public int pierceAmount = 2;
        public float pierceGravity = 0.1f;
        
        [Header("Spin Sword info")]
        public float maxSpinDistance = 7f;
        public float spinDuration = 1.5f;
        public float spinGravity = 0.1f;
        public float spinMoveSpeed = 2f;
        public float hitCoolDown = 0.35f;

        [Header("Aim dots")]
        public int numberOfDots = 20;
        public float spaceBetweenDots = 1;
        public GameObject dotPrefab;
        public Transform dotsParent;
    }
}