using System.Collections;
using UnityEngine;

public class BlobMovement : MonoBehaviour
{
    public float pushForce = 6f; // Force applied to move the blob
    public float rotationSpeed = 5f; // Speed of smooth rotation
    private Rigidbody rb;
    private Quaternion targetRotation; // Target rotation for the blob
    private float baseForce = 6f;
    private float sprintForce = 8f;
    private bool sprint = false;

    private bool canJump = true; // For optional jump cooldown

    void Start()
    {
        // Ensure the object has a Rigidbody for physics-based movement
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the object. Please add a Rigidbody.");
        }

        // Optional: Set Rigidbody constraints for blobby behavior
        rb.mass = 10f; // Adjust mass for blobby feel
        rb.drag = 10f; // Higher drag to simulate resistance
        rb.angularDrag = 10f; // Prevent excessive rotation

        targetRotation = transform.rotation; // Initialize target rotation
    }

    void Update()
    {
        // Get the camera's forward and right vectors (ignoring vertical tilt)
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Flatten the camera's forward and right vectors to stay on the X-Z plane
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Input for WASD keys
        Vector3 forceDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) // Forward
        {
            forceDirection += cameraForward; // Camera-relative forward
        }
        if (Input.GetKey(KeyCode.S)) // Backward
        {
            forceDirection -= cameraForward; // Camera-relative backward
        }
        if (Input.GetKey(KeyCode.A)) // Left
        {
            forceDirection -= cameraRight; // Camera-relative left
        }
        if (Input.GetKey(KeyCode.D)) // Right
        {
            forceDirection += cameraRight; // Camera-relative right
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //forceDirection += Vector3.up;
            Debug.Log("Space pressed.");
            
            JumpBlob(); // Call the jump function
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            forceDirection -= Vector3.up;
        }

        sprint = Input.GetKey(KeyCode.LeftShift);

        if (sprint) pushForce = sprintForce; else pushForce = baseForce;

        // Apply the force in FixedUpdate for consistent physics
        if (forceDirection != Vector3.zero)
        {
            // Rotate the blob to face the movement direction
            SetTargetRotation(Quaternion.LookRotation(forceDirection).eulerAngles.y);
            PushBlob(forceDirection.normalized);
        }

        // Smoothly rotate towards the target rotation
        SmoothRotate();
    }

    void PushBlob(Vector3 direction)
    {
        rb.AddForce(direction * pushForce, ForceMode.Impulse);
    }

    public void JumpBlob()
    {
        // OPTION 1: Simple upward force jump
        // if (IsGrounded())
        // {
        //     rb.AddForce(Vector3.up * pushForce * 2, ForceMode.Impulse);
        // }

        // OPTION 2: Jump with a cooldown
        
        // if (IsGrounded() && canJump)
        // {
        //     rb.AddForce(Vector3.up * pushForce * 2, ForceMode.Impulse);
        //     Debug.Log("Jump force applied.");
        //     StartCoroutine(JumpCooldown());
        // }
        

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * pushForce, ForceMode.Impulse);
        }
        
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(0.5f); // Adjust cooldown duration
        canJump = true;
    }

    private bool IsGrounded()
    {
        // Raycast downward to check for ground
        float groundCheckDistance = 0.1f;
        //return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        return true;
    }


    void SetTargetRotation(float yRotation)
    {
        // Set the target rotation to the specified Y-axis value
        targetRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void SmoothRotate()
    {
        // Smoothly interpolate the rotation towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
