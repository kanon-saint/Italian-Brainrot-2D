using UnityEngine;

public class AxeOrbit : MonoBehaviour
{
    public Transform character;       // The object to orbit around
    public float radius = 3f;       // Distance from the character
    public float orbitSpeed = 180f;   // Degrees per second (orbit around character)
    public float selfSpinSpeed = 360f; // Degrees per second (self-rotation)

    private float angle;

    void Update()
    {
        if (character == null) return;

        // Update orbit angle
        angle += orbitSpeed * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        // Calculate orbit position
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.position = character.position + offset;

        // Rotate the axe clockwise on its own axis
        transform.Rotate(Vector3.forward, -selfSpinSpeed * Time.deltaTime);
        // Use "-" to make it clockwise (Unity's Z axis is left-handed)
    }
}
