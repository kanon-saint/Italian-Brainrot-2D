using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int baseLaserDamage;
    [SerializeField] private float baseLaserDuration;
    [SerializeField] private float baseLaserCooldown;
    [SerializeField] private Vector3 positionOffset;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    private int laserDamage;
    private float laserDuration;
    private float laserCooldown;

    private void Start()
    {
        transform.position += positionOffset;

        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();

        ApplyWeaponLevelBehavior();
        StartCoroutine(LaserCycle());
    }

    private void ApplyWeaponLevelBehavior()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("WeaponData is not assigned to LaserTrigger.");
            return;
        }

        int level = weaponData.level;

        // You can fine-tune these values per level
        switch (level)
        {
            default:
                laserDamage = baseLaserDamage;
                laserDuration = baseLaserDuration;
                laserCooldown = baseLaserCooldown;
                break;
            case 2:
                laserDamage = baseLaserDamage + 2;
                laserDuration = baseLaserDuration + 0.5f;
                laserCooldown = baseLaserCooldown - 0.5f;
                break;
            case 3:
                laserDamage = baseLaserDamage + 3;
                laserDuration = baseLaserDuration + 0.75f;
                laserCooldown = baseLaserCooldown - 0.5f;
                break;
            case 4:
                laserDamage = baseLaserDamage + 5;
                laserDuration = baseLaserDuration + 1.5f;
                laserCooldown = baseLaserCooldown - 0.5f;
                break;
        }

        // Clamp cooldown to avoid going negative
        laserCooldown = Mathf.Max(0.5f, laserCooldown);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (laserCollider.enabled && other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(laserDamage);
                Debug.Log($"Enemy hit! Damage: {laserDamage}, Remaining HP: {enemyAttributes.health}");
            }
        }
    }

    private IEnumerator LaserCycle()
    {
        while (true)
        {
            laserCollider.enabled = true;
            if (laserRenderer != null) laserRenderer.enabled = true;

            yield return new WaitForSeconds(laserDuration);

            laserCollider.enabled = false;
            if (laserRenderer != null) laserRenderer.enabled = false;

            yield return new WaitForSeconds(laserCooldown);
        }
    }
}
