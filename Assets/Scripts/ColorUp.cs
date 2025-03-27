using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUp : MonoBehaviour
{
 public Material powerUpMaterial; // Assign the material in the Inspector
 public bool isHidden = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Navigate up to the parent of the Ball object (BLOB)
            Transform blobParent = other.transform.parent;

            if (blobParent != null)
            {
                // Find the BallMesh object within the BLOB parent
                Transform ballMeshTransform = blobParent.Find("BallMesh");

                if (ballMeshTransform != null)
                {
                    // Get the SkinnedMeshRenderer from the BallMesh object
                    SkinnedMeshRenderer meshRenderer = ballMeshTransform.GetComponent<SkinnedMeshRenderer>();

                    if (meshRenderer != null)
                    {
                        // Apply the new material to the SkinnedMeshRenderer
                        meshRenderer.material = powerUpMaterial;
                        Debug.Log($"Material successfully changed to: {powerUpMaterial.name}");
                    }
                    else
                    {
                        Debug.LogWarning("SkinnedMeshRenderer not found on BallMesh.");
                    }
                }
                else
                {
                    Debug.LogWarning("'BallMesh' object not found under the Player's parent.");
                }
            }
            else
            {
                Debug.LogWarning("Player object has no parent.");
            }

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
