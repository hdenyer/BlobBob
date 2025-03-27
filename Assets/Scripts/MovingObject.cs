using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 direction = Vector3.right; // Direction of movement (e.g., right, up, forward)
    public float distance = 5f; // Distance to move from the starting position
    public float speed = 2f; // Speed of movement
    public float pauseDuration = 1f; // Pause time at starting and ending positions

    private Vector3 startingPosition; // Original position of the object
    private Vector3 targetPosition; // Position to move to
    private bool movingForward = true; // Flag to determine movement direction
    private bool isPaused = false; // Flag to check if the object is currently pausing

    private void Start()
    {
        // Record the starting position
        startingPosition = transform.position;

        // Calculate the target position
        targetPosition = startingPosition + direction.normalized * distance;
    }

    private void Update()
    {
        if (!isPaused)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        // Determine the current destination
        Vector3 destination = movingForward ? targetPosition : startingPosition;

        // Move the object toward the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Check if the object has reached the destination
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            // Pause at the destination
            StartCoroutine(PauseBeforeMoving());
        }
    }

    private System.Collections.IEnumerator PauseBeforeMoving()
    {
        isPaused = true;

        // Wait for the specified pause duration
        yield return new WaitForSeconds(pauseDuration);

        // Reverse direction and resume movement
        movingForward = !movingForward;
        isPaused = false;
    }
}
