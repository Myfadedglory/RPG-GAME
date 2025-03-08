using System;
using UnityEngine;

namespace Script.Entity.Player
{
    [Serializable]
    public class PlayerConfig
    {
        [Header("Attack info")]
        public Vector2[] attackMovement;
        public float attackSpeed = 1f;
        public float comboWindow = 1f;

        [Header("Move info")]
        public float defaultMoveSpeed = 3.80f;
        public Vector2 defaultJumpForce = new (4, 8);

        [Header("Dash info")]
        public float defaultDashSpeed = 40;
        public float dashDuration = .2f;

        [Header("Hit info")]
        public float hitDuration = 0.2f;

        [Header("Counter Attack info")]
        public float counterAttackDuration = 0.1f;
        public float swordReturnForce = 7f;
    }
}