using Assets.Scripts.Scriptable_Objects;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

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
        Quaternion yawRotation = Quaternion.Euler(0, cameraOrientation.transform.eulerAngles.y, 0);
        MovePlayer(yawRotation);
        if (doJump)
            jump();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer(Quaternion yawRotation)
    {
        // Calculate movement direction
        moveDirection = yawRotation * Vector3.forward * verticalInput + yawRotation * Vector3.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        rb.velocity = new Vector3(rb.velocity.x * currentDrag, rb.velocity.y, rb.velocity.z * currentDrag);
    }

    private void groundCheck()
    {
        RaycastHit hit;

        //Create a boxcast from the center of the player towards the floor
        //grounded = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out hit,
        //   transform.rotation, playerHeight * 0.5f, whatIsGround);
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

            if( slopeAngle > 40f ) {
                slopeSlipping();
            }

            if (hit.distance > playerHeight * 0.5f)
                return;

            currentDrag = groundDrag;
            float lerpFactor = 10f * Time.deltaTime;
            if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, lerpFactor), rb.velocity.z);
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

    private void displayRaycastNormal(RaycastHit hit)
    {
        Vector3 incomingVec = hit.point - gameObject.transform.position;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
        Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
        Debug.DrawRay(hit.point, reflectVec, Color.green);
        Debug.DrawRay(transform.position, cameraOrientation.transform.forward, Color.blue);
    }

    private void slopeSlipping();
}