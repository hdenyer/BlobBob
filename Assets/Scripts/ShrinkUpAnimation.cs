using UnityEngine;

public class ShrinkUpAnimation : MonoBehaviour
{
    public float maxScale = 0f; // Maximum scale size
    public float animationDuration = 2f; // Time to grow and shrink

    private Vector3 originalScale; // Store the original scale of the object
    private float elapsedTime = 0f; // Track time for the animation
    private bool shrinking = false; // Whether the object is currently shrinking

    void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Increment elapsed time
        elapsedTime += 0.5f * Time.deltaTime;

        // Calculate the scale factor
        float scaleFactor;

        if (!shrinking)
        {
            // Growing phase
            scaleFactor = Mathf.Lerp(1f, maxScale, elapsedTime / animationDuration);

            // Switch to shrinking when peak is reached
            if (elapsedTime >= animationDuration)
            {
                shrinking = true;
                elapsedTime = 0f; // Reset elapsed time for shrinking phase
            }
        }
        else
        {
            // Shrinking phase
            scaleFactor = Mathf.Lerp(maxScale, 1f, elapsedTime / animationDuration);

            // Stop shrinking when back to original scale
            if (elapsedTime >= animationDuration)
            {
                shrinking = false;
                elapsedTime = 0f; // Reset elapsed time for growing phase
            }
        }

        // Apply the calculated scale factor
        transform.localScale = originalScale * scaleFactor;
    }
}
