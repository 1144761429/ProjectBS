using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 10f;
    private float moveSpeed = 0f;
    
    private Rigidbody rb;
    
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        // Get input from the horizontal and vertical axis (WASD or arrow keys by default)
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
    }

    private void FixedUpdate()
    {
        // Apply the movement to the Rigidbody
        rb.AddForce(moveVelocity * (Time.fixedDeltaTime * 300), ForceMode.VelocityChange);
    }
}
