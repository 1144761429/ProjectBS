using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 10f;
    private float moveSpeed = 0f;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    public float jumpForce = 10f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        // Get input from the horizontal and vertical axis (WASD or arrow keys by default)
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * moveSpeed;

        if (Input.GetButtonDown("Shift"))
        {
            moveSpeed = runSpeed;
        }
        if (Input.GetButtonUp("Shift"))
        {
            Thread thread = new Thread(() =>
            {
                moveSpeed = 0f;
                Thread.Sleep(100);
                moveSpeed = walkSpeed;
            });
            thread.Start();
        }

        if (isGrounded && Input.GetButtonDown("Jump")) // Default "Jump" is spacebar
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Apply the movement to the Rigidbody
        rb.AddForce(moveVelocity * Time.fixedDeltaTime * 300);
    }

    void OnCollisionEnter(Collision other)
    {
        //Check if the collision is with the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        //Check if the object leaving collision is the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    // TODO: CRTK hit
}
