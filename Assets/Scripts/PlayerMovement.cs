using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Varibles")]
    public float speed;
    public float groundDrag;
    public float playerHeight;
    public float jumpForce;
    public float jumpCooldown;
    public bool readyToJump;

    [Header("Inputs")]
    public float horizontal;
    public float vertical;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Components")]
    public Rigidbody rb;
    public Transform orientation;
    Vector3 moveDir;

    [Header("Ground Check")]
    public LayerMask WhatIsGround;
    public bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    void Update()
    {
        MyInput();
        SpeedControl();

        isGround = Physics.Raycast(transform.position, Vector3.down, playerHeight, WhatIsGround);

        if(isGround)
            rb.drag = groundDrag;

        else if(!isGround)
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        Movement();
    }

    public void MyInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && isGround)
        {
            Jump();

            readyToJump = false;

            Invoke("JumpReset", jumpCooldown);
        }
    }

    public void Movement()
    {
        moveDir = orientation.forward * vertical + orientation.right * horizontal;

        transform.rotation = orientation.rotation;

        rb.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > speed)
        {
            Vector3 maxVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(maxVel.x, rb.velocity.y, maxVel.z);
        }
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void JumpReset()
    {
        readyToJump = true;
    }

}
