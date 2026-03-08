using System;
using Assets.Scripts.Scriptable_Objects;
using Entities.Player;
using TMPro;
using UnityEngine;
using static Assets.Scripts.PlayerRelated.PlayerUtility;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Transform cameraOrientation;
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private Rigidbody rb;
        public PlayerDataSO playerData;
        
        [Header("Settings")]
        [SerializeField] private MovementSettings moveSettings;
        [SerializeField] private GroundSettings groundSettings;
        [SerializeField] private JumpSettings jumpSettings;
        
        // State Variables
        private bool isGrounded;
        private bool onSlope;
        private bool isJumping;
        private bool isInWater;
        private float jumpCooldownTimer;
        private int remainingJumps;
        
        // Physics cache
        private RaycastHit slopeHit;
        private Vector3 moveDirection;

        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            
            remainingJumps = jumpSettings.maxJumpCount;
        }

        #region Update Loops
        // Update is called once per frame, used to handle everything that is not directly related to player movement
        private void Update()
        {
            if(playerData != null) playerData.position = transform.position;
            
            if (jumpCooldownTimer > 0) jumpCooldownTimer -= Time.deltaTime;
            
            if (input.JumpPressed && jumpCooldownTimer <= 0 && remainingJumps > 0) PerformJump();
            
            UpdateDebugUI();
        }

        private void FixedUpdate()
        {
            PerformGroundAndSlopeChecks();
            CalculateMovementDirection();
            ApplyMovementPhysics();
            ApplyDrag();
            ControlSpeed();

            input.ClearInputTriggers();
        }
        #endregion
        
        #region Logic
        private void PerformGroundAndSlopeChecks()
        {
            // Ground Check
            bool wasGrounded = isGrounded;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, groundSettings.playerHeight * 0.5f + 0.2f, groundSettings.whatIsGround);

            // Slope Check
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, groundSettings.playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                onSlope = angle < groundSettings.maxSlopeAngle && angle != 0;
            }
            else
                onSlope = false;

            // Reset Jumps logic
            if (isGrounded && !isJumping) 
            {
                remainingJumps = jumpSettings.maxJumpCount;
            }

            // Landing Logic
            if (isGrounded && !wasGrounded && !isJumping) 
            {
                // Cancel vertical momentum on landing
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); 
            }
            
            // Reset Jumping flag if we are falling
            if (rb.linearVelocity.y < 0) isJumping = false;
        }

        private void CalculateMovementDirection()
        {
            Vector3 forward = cameraOrientation.forward;
            Vector3 right = cameraOrientation.right;
            
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * input.Vertical + right * input.Horizontal).normalized;
        }
        
        private void PerformJump()
        {
            isJumping = true;
            isGrounded = false;
            remainingJumps--;
            jumpCooldownTimer = jumpSettings.jumpCooldown;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.linearDamping = 0;
            
            rb.AddForce(transform.up * jumpSettings.jumpForce, ForceMode.Impulse);
        }
        
        private void ApplyMovementPhysics()
        {
            Vector3 slopeMoveDir = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;

            Vector3 forceToApply;
            float speedMultiplier = 10f;

            if (onSlope && !isJumping)
            {
                forceToApply = slopeMoveDir * moveSettings.moveSpeed * 20f; // Higher force on slopes to fight gravity
                if (rb.linearVelocity.y > 0) 
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force); // Stick to slope
            }
            else if (isGrounded)
            {
                forceToApply = moveDirection * moveSettings.moveSpeed * speedMultiplier;
            }
            else // In Air
            {
                forceToApply = moveDirection * moveSettings.moveSpeed * speedMultiplier * moveSettings.airMultiplier;
            }

            rb.AddForce(forceToApply, ForceMode.Force);
            rb.useGravity = !onSlope;
        }
        
        private void ApplyDrag()
        {
            if (isJumping) rb.linearDamping = 0;
            else if (isGrounded) rb.linearDamping = moveSettings.groundDrag;
            else rb.linearDamping = moveSettings.airDrag;
        }
        
        private void ControlSpeed()
        {
            // On Slope: Limit total magnitude
            if (onSlope && !isJumping)
            {
                if (rb.linearVelocity.magnitude > moveSettings.moveSpeedCap)
                    rb.linearVelocity = rb.linearVelocity.normalized * moveSettings.moveSpeedCap;
            }
            // On Ground/Air: Limit only X/Z, allow Gravity (Y) to accelerate freely
            else
            {
                Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                if (flatVel.magnitude > moveSettings.moveSpeedCap)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSettings.moveSpeedCap;
                    rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
                }
            }
        }

        private void UpdateDebugUI()
        {
            if (debugText != null)
                debugText.text = $"Vel: {rb.linearVelocity.magnitude:F1} | Slope: {onSlope} | Ground: {isGrounded}";
        }
        #endregion
        
        #region Trigger Events
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("water")) isInWater = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("water")) isInWater = false;
        }
        #endregion
    }
}