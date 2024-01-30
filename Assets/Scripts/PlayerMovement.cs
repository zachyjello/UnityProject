using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int jumpCooldown;
    public float jumpForce;
    float horizontalInput;
    float verticalInput;
    boolean 

    Vector3 moveDirection;

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
        MyInput();
        groundCheck();
        jumpCommandCheck();
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
        if(curJumpCount > 0){
            curJumpCount--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }
        if(grounded)
            curJumpCount = maxJumpCount;
    }
    private void jumpCommandCheck(){
        if(Input.GetKey("space"))
            doJump = true;
    }
}
