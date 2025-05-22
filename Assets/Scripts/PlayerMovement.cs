using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private Vector2 movement;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal movement
        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        // Apply movement using Transform
        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;

        // Animator
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip toward mouse cursor
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseWorldPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }
}
