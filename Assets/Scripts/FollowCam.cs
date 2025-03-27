using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of smooth rotation
    public float distance = 10f; // Distance from the blob
    public float heightOffset = 5f; // Base height offset for the camera
    public float verticalRotationSpeed = 50f; // Speed of vertical rotation
    public float minVerticalAngle = -20f; // Minimum vertical angle (to prevent flipping)
    public float maxVerticalAngle = 80f; // Maximum vertical angle (to prevent overhead flipping)

    private Transform blob; // Reference to the blob's transform
    private float currentHorizontalAngle = 0f; // Current horizontal angle around the blob
    private float currentVerticalAngle = 20f; // Current vertical angle (starting at a slight elevation)

    void Start()
    {
        blob = transform; // The script is attached to the blob
    }

    void Update()
    {
        // Adjust horizontal angle based on left and right arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentHorizontalAngle -= rotationSpeed * Time.deltaTime * 50f; // Increment left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentHorizontalAngle += rotationSpeed * Time.deltaTime * 50f; // Increment right
        }

        // Adjust vertical angle based on up and down arrow keys
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentVerticalAngle += verticalRotationSpeed * Time.deltaTime; // Increment up
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentVerticalAngle -= verticalRotationSpeed * Time.deltaTime; // Increment down
        }

        // Clamp the vertical angle to prevent flipping
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

        // Smoothly update the camera's position and rotation
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        if (blob == null) return;

        // Convert horizontal and vertical angles to radians
        float horizontalRadians = currentHorizontalAngle * Mathf.Deg2Rad;
        float verticalRadians = currentVerticalAngle * Mathf.Deg2Rad;

        // Calculate the camera's offset using spherical coordinates
        float xOffset = Mathf.Sin(horizontalRadians) * Mathf.Cos(verticalRadians) * distance;
        float zOffset = Mathf.Cos(horizontalRadians) * Mathf.Cos(verticalRadians) * distance;
        float yOffset = Mathf.Sin(verticalRadians) * distance + heightOffset;

        Vector3 targetPosition = blob.position + new Vector3(xOffset, yOffset, zOffset);

        // Smoothly move the camera to the new position
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * rotationSpeed);

        // Ensure the camera looks at the blob
        Camera.main.transform.LookAt(blob.position);
    }
}
