using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float accelSpeed = 90f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float gravity = 25f;
    float x;

    [Header("Don't Change")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    float groundCheckRadius = 0.1f;
    bool isGrounded;

    float coyoteTimeCounter;
    float coyoteTime = 0.1f;
    float jumpBufferCounter;
    float jumpBuffer = 0.1f;
    float jumpTimeCounter;
    float jumpTime = 0.2f;


    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
    }


    void FixedUpdate()
    {
        Jump();
        rb.velocity += new Vector2(x * accelSpeed * Time.fixedDeltaTime, 0f);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded || coyoteTimeCounter > 0f)
        {
            jumpTimeCounter = jumpTime;
        }
        else if (!isGrounded && !Input.GetButton("Jump"))
        {
            jumpTimeCounter = 0f;
        }


        // Limit speed.
        if (Math.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed * x, rb.velocity.y);
        }


        if (x == 0)
        {
            if (Math.Abs(rb.velocity.x) > 0.2f)
            {
                if (isGrounded) { rb.velocity = new Vector2(rb.velocity.x / 1.25f, rb.velocity.y); }
                else { rb.velocity = new Vector2(rb.velocity.x / 1.01f, rb.velocity.y); }
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }


        rb.velocity -= new Vector2(0f, 2f * gravity * Time.fixedDeltaTime);

    }


    void Jump()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if (Input.GetButton("Jump"))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }


        if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || (Input.GetButton("Jump") && jumpTimeCounter > 0f))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
    }
}
