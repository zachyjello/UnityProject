using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;

    [Header("Ground Check")]
    public Transform orientation;
    public LayerMask whatIsGround;
    public float playerHeight;
    public bool grounded;

    [Header("Jumping")]
    public int curJumpCount;
    public int maxJumpCount;
    public float jumpCooldown;
    public float jumpTimer;
    public float jumpForce;
    float horizontalInput;
    float verticalInput;
    private bool doJump = false;
    private bool hasJumped = false;

    Vector3 moveDirection;

    public PlayerDataSO playerData;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

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
        if(doJump)
            jump();
    }

    private void MovePlayer(){
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void groundCheck(){
        RaycastHit hit;
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        grounded = Physics.BoxCast(transform.position, new Vector3(0.5f,0.5f,0.5f), Vector3.down, out hit, transform.rotation, playerHeight * 0.5f + 0.2f, whatIsGround);
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void jump(){
        rb.AddForce(Vector3.up * jumpForce);
    }
    private void jumpCommandCheck(){
        if(grounded){
            curJumpCount = maxJumpCount;
            hasJumped = false;
        }
        if(curJumpCount > 0 && jumpCooldown<=0 && Input.GetKeyDown("space")){
            curJumpCount--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
            jumpCooldown = jumpTimer;
            doJump = true;
            hasJumped = true;
        }
        else{
            doJump = false;
            jumpCooldown-=Time.deltaTime;
        }
    }
    private void noSlipping(){
        if(!hasJumped)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
