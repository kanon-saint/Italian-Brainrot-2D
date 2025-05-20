using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private Renderer backgroundRenderer; // Assign your background's Renderer (e.g. a Quad with a material)

    private float offset;

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        backgroundRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
