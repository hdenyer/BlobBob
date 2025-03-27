using System.Collections;
using UnityEngine;

public class ShrinkUp : MonoBehaviour
{
    public float newScale = 50f; // The new scale for the blob upon collision
    public bool isHidden = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the power-up is the blob
        if (other.CompareTag("Player") && !isHidden)
        {
            // Scale the blob
            other.transform.localScale = Vector3.one * newScale;

            // Disable the power-up visually and physically
            HidePowerUp();

            // Start the timer to reset the power-up
            StartCoroutine(ResetPowerUpAfterDelay(15f));
        }
    }

    void HidePowerUp()
    {
        // Disable all Renderers to make the object and its children invisible
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // Disable the Collider to prevent further collisions
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        isHidden = true;
    }

    IEnumerator ResetPowerUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Re-enable the power-up visually and physically
        ResetPowerUp();
    }

    void ResetPowerUp()
    {
        // Enable all Renderers to make the object and its children visible
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        // Enable the Collider to allow collisions
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        isHidden = false;
    }
}
