using System;
using Assets.Scripts.Scriptable_Objects;
using TMPro;
using UnityEngine;
using static Assets.Scripts.PlayerRelated.PlayerUtility;

namespace PlayerRelated
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] public float moveSpeed;
        public float moveSpeedCap;
        public float groundDrag;
        public float airDrag;
        public bool isInWater;

        [Header("Ground Check")] public Transform cameraOrientation;
        public LayerMask whatIsGround;
        public float playerHeight;
        public bool grounded;
        public float minGroundDotProduct = 1;
        public int stepsSinceLastGrounded;

        [Header("Slope Control")] public float maxSlopeAngle;

        [Header("Jumping")] public int curJumpCount;

        public int maxJumpCount;
        public float jumpCooldown;
        public float jumpTimer;
        public float jumpForce;

        public PlayerDataSO playerData;

        public playerMovementState movementState;
        public playerMovementStatus movementStatus;

        public TextMeshProUGUI elementToDisplay;
        private Vector3 contactNormal;
        private float currentDrag;
        private bool doJump;


        private float horizontalInput;
        private bool jumpInput;

        private Vector3 moveDirection;

        private Rigidbody rb;
        private RaycastHit slopeHit;
        private Transform usableOrientation;

        private float verticalInput;

        // Start is called before the first frame update
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        // Update is called once per frame, used to handle everything that is not directly related to player movement
        private void Update()
        {
            //Updates position in player data scriptable object
            playerData.position = transform.position;

            //Checks for inputs, and grounded
            MyInput();
            DragControl();
            playerJumpCheck();
            SpeedControl();

            elementToDisplay.text = $"Velocity: {rb.velocity.magnitude.ToString()} \nOnSlope: {OnSlope()} \nGrounded: {grounded}";
        }

        private void FixedUpdate()
        {
            var yawRotation = Quaternion.Euler(0, cameraOrientation.transform.eulerAngles.y, 0);
            groundCheck();
            MovePlayer(yawRotation);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("water")) isInWater = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("water")) isInWater = false;
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            jumpInput = Input.GetKeyDown("space");
        }

        private void MovePlayer(Quaternion yawRotation)
        {
            moveDirection = (yawRotation * Vector3.forward * verticalInput + yawRotation * Vector3.right * horizontalInput).normalized;

            if (OnSlope()) // Only apply slope logic when actually grounded
            {
                rb.AddForce(GetSlopeDirectionNormal() * moveSpeed * 20f, ForceMode.Force);

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            }

            //rb.velocity = new Vector3(rb.velocity.x * currentDrag, rb.velocity.y, rb.velocity.z * currentDrag);
            //rb.drag = currentDrag;
            // Only disable gravity when grounded AND on slope
            rb.useGravity = !OnSlope();
        }

        private void DragControl()
        {
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = airDrag;
        }

        private void groundCheck()
        {
            RaycastHit hit;
            bool wasGrounded = grounded;

            grounded = Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.1f, whatIsGround);

            if (grounded)
            {
                DisplayRaycastNormal(hit);

                var playerVector = transform.up;
                var slopeAngle = Vector3.Angle(playerVector, hit.normal);

                if (hit.distance > playerHeight * 0.5f)
                    return;

                // Just landed (wasn't grounded last frame, now is)
                if (!wasGrounded && rb.velocity.y <= 0)
                {
                    // Immediately stop all downward movement when landing
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                    // If landing on a slope, also stop horizontal sliding
                    if (slopeAngle > 1f && slopeAngle < maxSlopeAngle)
                    {
                        // Project current velocity onto the slope to prevent sliding
                        Vector3 slopeParallel = Vector3.ProjectOnPlane(rb.velocity, hit.normal);
                        if (Vector3.Dot(slopeParallel.normalized, Vector3.down) > 0) // If sliding down
                        {
                            rb.velocity = Vector3.zero; // Stop all movement briefly
                        }
                    }
                }
                // Already grounded, just smooth out any remaining downward velocity
                else if (rb.velocity.y < 0)
                {
                    var lerpFactor = 20f * Time.deltaTime;
                    rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, lerpFactor), rb.velocity.z);
                }
            }
        }

        private void playerJumpCheck()
        {
            if (grounded)
            {
                curJumpCount = maxJumpCount;
            }

            if (curJumpCount > 0 && jumpCooldown <= 0 && jumpInput)
            {
                curJumpCount--;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); //Cancel current vertical velocity
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCooldown = jumpTimer;
                doJump = true;
            }
            else
            {
                doJump = false;
                jumpCooldown -= Time.deltaTime;
            }
        }

        private void DisplayRaycastNormal(RaycastHit hit)
        {
            var incomingVec = hit.point - gameObject.transform.position;
            var reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);
            Debug.DrawRay(transform.position, cameraOrientation.transform.forward, Color.blue);
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private void SpeedControl()
        {
            if (OnSlope())
            {
                if (rb.velocity.magnitude > moveSpeedCap) rb.velocity = rb.velocity.normalized * moveSpeedCap;
            }

            else
            {
                var flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                Debug.Log($"FlatVel: {flatVel.magnitude}");
                if (flatVel.magnitude > moveSpeedCap)
                {
                    var limitedVel = flatVel.normalized * moveSpeedCap;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                    Debug.Log("PEnis");
                }
            }
        }

        private Vector3 GetSlopeDirectionNormal()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
    }
}