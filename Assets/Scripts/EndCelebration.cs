using UnityEngine;

public class EndCelebration : MonoBehaviour
{
    public GameObject[] softbodyObjects; // Assign the softbody objects in the Inspector
    public float upwardForceMagnitude = 10f; // Strength of the upward force
    public float downwardForceMagnitude = 10f; // Strength of the downward force
    public float randomizedFactor = 2f; // Max random variation in force magnitude
    public float forceDuration = 1f; // Duration for each force phase (upward/downward)

    private bool isTriggered = false; // Prevents re-triggering multiple times

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered) // Ensure the blob is tagged "Player"
        {
            isTriggered = true; // Set triggered state
            Debug.Log("Blob entered the trigger area. Applying repeating forces to softbodies.");
            
            // Start the repeating force application
            foreach (GameObject softbody in softbodyObjects)
            {
                if (softbody.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    StartCoroutine(ApplyRepeatingForces(rb));
                }
                else
                {
                    Debug.LogWarning($"{softbody.name} does not have a Rigidbody component.");
                }
            }
        }
    }

    private System.Collections.IEnumerator ApplyRepeatingForces(Rigidbody rb)
    {
        while (true) // Repeat indefinitely
        {
            // Apply upward force
            yield return ApplyForce(rb, Vector3.up, upwardForceMagnitude);

            // Apply downward force
            yield return ApplyForce(rb, Vector3.down, downwardForceMagnitude);
        }
    }

    private System.Collections.IEnumerator ApplyForce(Rigidbody rb, Vector3 direction, float baseMagnitude)
    {
        float elapsedTime = 0f;

        // Randomize the force magnitude slightly for dynamic motion
        float randomizedMagnitude = baseMagnitude + Random.Range(-randomizedFactor, randomizedFactor);

        while (elapsedTime < forceDuration)
        {
            rb.AddForce(direction.normalized * randomizedMagnitude, ForceMode.Acceleration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
