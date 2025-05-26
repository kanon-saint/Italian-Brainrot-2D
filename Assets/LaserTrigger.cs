using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private int laserDamage;
    [SerializeField] private float laserDuration = 2f;
    [SerializeField] private float laserCoolDown = 3f;

    // Use the offset similar to WeaponManager's previous logic but fixed
    [SerializeField] private Vector3 positionOffset = new Vector3(-11f, 0f, 0f);

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    private void Start()
    {
        // Apply the offset once when the laser starts
        transform.position += positionOffset;

        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LaserCycle());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (laserCollider.enabled && other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(laserDamage);
                Debug.Log("Enemy hit! Remaining HP: " + enemyAttributes.health);
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

            yield return new WaitForSeconds(laserCoolDown);
        }
    }
}
