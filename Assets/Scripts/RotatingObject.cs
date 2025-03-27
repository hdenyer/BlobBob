using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second
    public Vector3 rotationAxis = Vector3.up;
    private void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

}
