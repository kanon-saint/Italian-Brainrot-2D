using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private int laserDamage;
    [SerializeField] private float laserDuration = 2f;
    [SerializeField] private float laserCoolDown = 3f;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer; // Optional: to show/hide laser visually

    private void Start()
    {
        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>(); // if you want to toggle visibility
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
            // Activate laser
            laserCollider.enabled = true;
            if (laserRenderer != null) laserRenderer.enabled = true;

            yield return new WaitForSeconds(laserDuration);

            // Deactivate laser
            laserCollider.enabled = false;
            if (laserRenderer != null) laserRenderer.enabled = false;

            yield return new WaitForSeconds(laserCoolDown);
        }
    }
}
