using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the fireball
    public float maxTravelDistance = 15f; // The maximum distance the fireball can travel

    private Vector2 spawnPosition;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Fireball: Rigidbody2D component not found!", this);
        }
    }

    void Start()
    {
        spawnPosition = transform.position;

        // 🔁 Generate a random direction in 360 degrees
        float angle = Random.Range(0f, 360f);
        float radians = angle * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        if (rb != null)
        {
            // 🔁 Use velocity (or linearVelocity if you prefer)
            rb.linearVelocity = moveDirection * moveSpeed;
        }

        // 🔁 Optionally rotate the fireball sprite to match direction
        float angleDegrees = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
    }

    void Update()
    {
        float distanceTraveled = Vector2.Distance(transform.position, spawnPosition);
        if (distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
