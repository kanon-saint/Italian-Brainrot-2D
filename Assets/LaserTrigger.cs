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

        laserDamage = baseLaserDamage;
        laserDuration = baseLaserDuration;
        laserCooldown = baseLaserCooldown;

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
            case 1:
                // base values already assigned, so this is technically redundant
                break;
            case 2:
                laserDuration += 0.25f;
                laserCooldown -= 0.5f;
                break;
            case 3:
                laserDuration += 0.5f;
                laserCooldown -= 0.5f;
                break;
            case 4:
                laserDamage += 2;
                laserDuration += 1f;
                laserCooldown -= 0.5f;
                break;
            default:
                if (level > 4)
                {
                    laserDamage += 2;
                    laserDuration += (level - 4) * 0.2f;
                    laserCooldown -= (level - 4) * 0.1f;

                    // Clamp cooldown to avoid going negative
                    laserCooldown = Mathf.Max(0.5f, laserCooldown);
                }
                break;
        }
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
