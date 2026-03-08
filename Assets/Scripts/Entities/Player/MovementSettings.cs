using System;
using UnityEngine;

namespace Entities.Player
{
    // By moving these to their own file, they are accessible anywhere in the
    // "Entities.Player" namespace, but they don't clutter your main class.
    
    [Serializable]
    public struct MovementSettings
    {
        public float moveSpeed;
        public float moveSpeedCap;
        public float groundDrag;
        public float airDrag;
        [Range(0, 1)] public float airMultiplier;
    }

    [Serializable]
    public struct GroundSettings
    {
        public LayerMask whatIsGround;
        public float playerHeight;
        public float maxSlopeAngle;
    }

    [Serializable]
    public struct JumpSettings
    {
        public float jumpForce;
        public float jumpCooldown;
        public int maxJumpCount;
    }
}