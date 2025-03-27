using UnityEngine;

public class ColorDoor : MonoBehaviour
{
    public Material doorMaterial; // Assign the material in the Inspector
    public float colliderDisableTime = 2f; // Time for which the collider is disabled

    private Renderer doorRenderer;
    private Collider doorCollider;

    private void Start()
    {
        // Cache references to the door's Renderer and Collider
        doorRenderer = GetComponent<Renderer>();
        doorCollider = GetComponent<Collider>();

        // Assign the material to the door's renderer
        if (doorRenderer != null)
        {
            doorRenderer.material = doorMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && IsBlobMaterialMatching(collision.collider))
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    private bool IsBlobMaterialMatching(Collider playerCollider)
    {
        // Locate the BallMesh object under the Player hierarchy
        Transform blobParent = playerCollider.transform.parent;
        Transform ballMeshTransform = blobParent?.Find("BallMesh");

        if (ballMeshTransform == null) return false;

        // Get the SkinnedMeshRenderer from the BallMesh object
        SkinnedMeshRenderer meshRenderer = ballMeshTransform.GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer == null) return false;

        // Compare blob material to the door material
        Material blobMaterial = meshRenderer.material;
        return blobMaterial == doorMaterial || CleanMaterialName(blobMaterial.name) == CleanMaterialName(doorMaterial.name);
    }

    private string CleanMaterialName(string materialName)
    {
        // Remove "(Instance)" tag from the material name
        return materialName.Replace(" (Instance)", "").Trim();
    }

    private System.Collections.IEnumerator DisableColliderTemporarily()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
            yield return new WaitForSeconds(colliderDisableTime);
            doorCollider.enabled = true;
        }
    }
}
