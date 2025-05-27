using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    [Header("Base Stats")]
    [SerializeField] private int baseLaserDamage;
    [SerializeField] private float baseLaserDuration;
    [SerializeField] private float baseLaserCooldown;
    [SerializeField] private Vector3 positionOffset;

    [Header("Level Scaling")]
    [SerializeField] private int damagePerLevel = 2;
    [SerializeField] private float durationPerLevel = 0.2f;
    [SerializeField] private float cooldownReductionPerLevel = 0.1f;

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip laserSound;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    // Dynamic Properties
    private int Level => Mathf.Max(1, weaponData != null ? weaponData.level : 1);
    private int LaserDamage => baseLaserDamage + (Level - 1) * damagePerLevel;
    private float LaserDuration => baseLaserDuration + (Level - 1) * durationPerLevel;
    private float LaserCooldown => Mathf.Max(0.5f, baseLaserCooldown - (Level - 1) * cooldownReductionPerLevel);

    private void Start()
    {
        transform.position += positionOffset;

        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();

        Debug.Log($"[LaserTrigger] Initialized at Level {Level} with Damage {LaserDamage}, Duration {LaserDuration}, Cooldown {LaserCooldown}");

        StartCoroutine(LaserCycle());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (laserCollider.enabled && other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(LaserDamage);
                Debug.Log($"Laser hit enemy with damage: {LaserDamage}");
            }
        }
    }

    private IEnumerator LaserCycle()
    {
        while (true)
        {
            PlayLaserSound();

            laserCollider.enabled = true;
            if (laserRenderer != null) laserRenderer.enabled = true;

            yield return new WaitForSeconds(LaserDuration);

            laserCollider.enabled = false;
            if (laserRenderer != null) laserRenderer.enabled = false;

            yield return new WaitForSeconds(LaserCooldown);
        }
    }

    private void PlayLaserSound()
    {
        if (audioSource != null && laserSound != null)
        {
            audioSource.clip = laserSound;
            audioSource.Play();
        }
    }
}
