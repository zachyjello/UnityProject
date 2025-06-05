using Assets.Scripts.Scriptable_Objects;
using UnityEngine;
using static Assets.Scripts.PlayerRelated.PlayerUtility;

namespace Assets.Scripts.PlayerRelated{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] public float moveSpeed;

        public float groundDrag;
        public float airDrag;

        [Header("Ground Check")] public Transform cameraOrientation;
        private Transform usableOrientation;

        public LayerMask whatIsGround;
        public float playerHeight;
        public bool grounded;

        [Header("Jumping")] public int curJumpCount;

        public int maxJumpCount;
        public float jumpCooldown;
        public float jumpTimer;
        public float jumpForce;

        public PlayerDataSO playerData;
        private float currentDrag;
        private bool doJump;
        private bool hasJumped;
        private float horizontalInput;
        private float verticalInput;
        private bool jumpInput;

        private Vector3 moveDirection;

        private Rigidbody rb;

        public playerMovementState movementState;
        public playerMovementStatus movementStatus;

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
            grounded = groundCheck();
            dragControl(grounded);
            PlayerJumpCheck();
        }

        private void FixedUpdate()
        {
            Quaternion yawRotation = Quaternion.Euler(0, cameraOrientation.transform.eulerAngles.y, 0);
            MovePlayer(yawRotation);
            if (doJump)
                jump();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            jumpInput = Input.GetKeyDown("space");
        }

        private void MovePlayer(Quaternion yawRotation)
        {
            // Calculate movement direction
            moveDirection = yawRotation * Vector3.forward * verticalInput + yawRotation * Vector3.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            rb.velocity = new Vector3(rb.velocity.x * currentDrag, rb.velocity.y, rb.velocity.z * currentDrag);
        }

        private void dragControl(bool grounded)
        {
            if (grounded)
                currentDrag = groundDrag;
            else
                currentDrag = airDrag;
        }

        private bool groundCheck()
        {
            RaycastHit hit;

            grounded = Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f * 2, whatIsGround);

            Vector3 cameraForward = cameraOrientation.transform.forward;
            Vector3 forwardFacing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            Debug.DrawRay(transform.position, forwardFacing * 3f, Color.black);

            if (grounded)
            {
                displayRaycastNormal(hit);

                Vector3 playerVector = transform.up;
                float slopeAngle = Vector3.Angle(playerVector, hit.normal);
                Debug.Log($"Slope Angle = {slopeAngle}");

                if (slopeAngle > 40f)
                {
                    //slopeSlipping();
                }

                if (hit.distance > playerHeight * 0.5f)
                    return;

                float lerpFactor = 10f * Time.deltaTime;
                if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, lerpFactor), rb.velocity.z);
            }
        }

        private void jump()
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        private void PlayerJumpCheck()
        {
            if (grounded)
            {
                curJumpCount = maxJumpCount;
                hasJumped = false;
            }

            if (curJumpCount > 0 && jumpCooldown <= 0 && jumpInput)
            {
                curJumpCount--;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); //Cancel current vertical velocity
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCooldown = jumpTimer;
                doJump = true;
                hasJumped = true;
            }
            else
            {
                doJump = false;
                jumpCooldown -= Time.deltaTime;
            }
        }

        private void noSlipping()
        {
            if (!hasJumped)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        private void displayRaycastNormal(RaycastHit hit)
        {
            Vector3 incomingVec = hit.point - gameObject.transform.position;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, reflectVec, Color.green);
            Debug.DrawRay(transform.position, cameraOrientation.transform.forward, Color.blue);
        }

    }
}