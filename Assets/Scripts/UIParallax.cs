using UnityEngine;

public class AutoParallax : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;          // How fast background scrolls left
    [SerializeField] private float spriteWidth = 20f;          // Width of the sprite in world units
    [SerializeField] private Transform[] backgroundParts;      // Assign the two or more background pieces here

    private void Update()
    {
        // Move all background parts left
        foreach (var part in backgroundParts)
        {
            part.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // If part moved completely left off screen, move it to the right end
            if (part.position.x <= -spriteWidth)
            {
                // Find rightmost part's X position
                float rightMostX = FindRightmostPartX();
                part.position = new Vector3(rightMostX + spriteWidth, part.position.y, part.position.z);
            }
        }
    }

    private float FindRightmostPartX()
    {
        float maxX = float.MinValue;
        foreach (var part in backgroundParts)
        {
            if (part.position.x > maxX)
                maxX = part.position.x;
        }
        return maxX;
    }
}
