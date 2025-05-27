using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int baseLaserDamage;
    [SerializeField] private float baseLaserDuration;
    [SerializeField] private float baseLaserCooldown;
    [SerializeField] private Vector3 positionOffset;

    // How much each level improves the laser
    [SerializeField] private int damagePerLevel = 1;
    [SerializeField] private float durationPerLevel = 0.2f;
    [SerializeField] private float cooldownReductionPerLevel = 0.1f;

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

        int level = Mathf.Max(1, weaponData.level); // Ensure level is at least 1

        // Apply scalable improvements
        laserDamage = baseLaserDamage + (level - 1) * damagePerLevel;
        laserDuration = baseLaserDuration + (level - 1) * durationPerLevel;
        laserCooldown = baseLaserCooldown - (level - 1) * cooldownReductionPerLevel;

        // Clamp cooldown to a safe minimum
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
