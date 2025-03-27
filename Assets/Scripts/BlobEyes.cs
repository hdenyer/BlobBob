using UnityEngine;

public class BlobEyes : MonoBehaviour
{
    public Transform eye1;
    public Transform eye2;

    private Vector3 eye1TargetPosition;
    private Vector3 eye2TargetPosition;

    // Corrected Positions
    private Vector3 eye1HomePosition = new Vector3(0.0015f, 0f, -0.006f);
    private Vector3 eye2HomePosition = new Vector3(-0.0015f, 0f, -0.006f);
    private Vector3 eye1ForwardPosition = new Vector3(0.0015f, 0f, 0.006f);
    private Vector3 eye2ForwardPosition = new Vector3(-0.0015f, 0f, 0.006f);
    private Vector3 eye1LeftPosition = new Vector3(0.0015f, 0f, -0.006f);
    private Vector3 eye2LeftPosition = new Vector3(-0.0015f, -0.009575f, 0.006f);
    private Vector3 eye1RightPosition = new Vector3(0.0015f, -0.009575f, -0.006f);
    private Vector3 eye2RightPosition = new Vector3(-0.0015f, 0f, 0.006f);

    public float moveSpeed = 3f; // Speed of eye movement

    void Start()
    {
        // Initialize target positions to home positions
        eye1TargetPosition = eye1HomePosition;
        eye2TargetPosition = eye2HomePosition;
    }

    void Update()
    {
        // Get the camera's forward and right vectors (flattened)
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Detect input and update target positions based on camera-relative direction
        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) // Forward
        {
            movementDirection += cameraForward;
        }
        if (Input.GetKey(KeyCode.S)) // Backward
        {
            movementDirection -= cameraForward;
        }
        if (Input.GetKey(KeyCode.A)) // Left
        {
            movementDirection -= cameraRight;
        }
        if (Input.GetKey(KeyCode.D)) // Right
        {
            movementDirection += cameraRight;
        }

        if (movementDirection != Vector3.zero)
        {
            UpdateEyeTargetPositions(movementDirection);
        }

        // Smoothly move eyes toward target positions
        MoveEyes();
    }

    void UpdateEyeTargetPositions(Vector3 movementDirection)
    {
        // Determine the primary direction of movement
        movementDirection = movementDirection.normalized;

        // Check which direction is most dominant
        if (Vector3.Dot(movementDirection, Vector3.forward) > 0.7f)
        {
            SetTargetPositions(eye1ForwardPosition, eye2ForwardPosition);
        }
        else if (Vector3.Dot(movementDirection, Vector3.back) > 0.7f)
        {
            SetTargetPositions(eye1HomePosition, eye2HomePosition);
        }
        else if (Vector3.Dot(movementDirection, Vector3.left) > 0.7f)
        {
            SetTargetPositions(eye1LeftPosition, eye2LeftPosition);
        }
        else if (Vector3.Dot(movementDirection, Vector3.right) > 0.7f)
        {
            SetTargetPositions(eye1RightPosition, eye2RightPosition);
        }
    }

    void SetTargetPositions(Vector3 eye1Pos, Vector3 eye2Pos)
    {
        eye1TargetPosition = eye1Pos;
        eye2TargetPosition = eye2Pos;
    }

    void MoveEyes()
    {
        if (eye1 != null)
        {
            eye1.localPosition = Vector3.Lerp(eye1.localPosition, eye1TargetPosition, Time.deltaTime * moveSpeed);
        }

        if (eye2 != null)
        {
            eye2.localPosition = Vector3.Lerp(eye2.localPosition, eye2TargetPosition, Time.deltaTime * moveSpeed);
        }
    }
}
