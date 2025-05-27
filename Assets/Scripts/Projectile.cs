using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private WeaponData weaponData;

    [Header("Damage Settings")]
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private int damagePerLevel = 2;

    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;

    private Rigidbody2D rb;

    // Dynamically calculated damage
    public int AttackDamage
    {
        get
        {
            if (weaponData == null) return baseDamage;
            int level = Mathf.Max(1, weaponData.level);
            return baseDamage + (level - 1) * damagePerLevel;
        }
    }

    public void SetDirection(Vector3 dir)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = dir.normalized * speed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log($"[Projectile] Attack damage: {AttackDamage} (Level {weaponData?.level ?? 0})");
    }

    void Update()
    {
        // Destroy projectile if it goes offscreen
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(AttackDamage);
                Debug.Log($"Projectile hit enemy with damage: {AttackDamage}");
            }

            // Destroy(gameObject); // Optional: destroy projectile on hit
        }
    }
}
