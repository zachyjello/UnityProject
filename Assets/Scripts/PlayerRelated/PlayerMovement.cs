using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] public float moveSpeed;

    public float groundDrag;
    public float airDrag;

    [Header("Ground Check")] public Transform orientation;

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

    private Vector3 moveDirection;

    private Rigidbody rb;

    private float verticalInput;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        playerData.position = transform.position;
        MyInput();
        groundCheck();
        jumpCommandCheck();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (doJump)
            jump();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        rb.velocity = new Vector3(rb.velocity.x * currentDrag, rb.velocity.y, rb.velocity.z * currentDrag);
    }

    private void groundCheck()
    {
        RaycastHit hit;
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        grounded = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out hit,
            transform.rotation, playerHeight * 0.5f, whatIsGround);
        if (grounded)
        {
            currentDrag = groundDrag;
            if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, 0.15f), rb.velocity.z);
        }
        else
        {
            currentDrag = airDrag; 
        }
    }

    private void jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
    }

    private void jumpCommandCheck()
    {
        if (grounded)
        {
            curJumpCount = maxJumpCount;
            hasJumped = false;
        }

        if (curJumpCount > 0 && jumpCooldown <= 0 && Input.GetKeyDown("space"))
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
}