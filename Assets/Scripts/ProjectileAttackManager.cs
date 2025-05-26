using UnityEngine;

public class ProjectileAttackManager : MonoBehaviour
{
    [SerializeField] private GameObject defaultAttackPfx;
    [SerializeField] private float attackDuration = 0.25f;
    [SerializeField] private float attackCooldown = 1f;
    private GameObject currentAttackPfx;

    private bool attacking = false;
    private float attackTimer = 0f;
    private float durationTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            ProjectileAttack();
            attackTimer = 0f;
        }

        if (attacking)
        {
            durationTimer += Time.deltaTime;

            if (durationTimer >= attackDuration)

                if (currentAttackPfx != null)
                {
                    Destroy(currentAttackPfx);
                }

            attacking = false;
            durationTimer = 0f;
        }
    }
    private void ProjectileAttack()
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        // If projectile prefab is set, instantiate and shoot it
        if (defaultAttackPfx != null)
        {
            GameObject projectile = Instantiate(defaultAttackPfx, transform.position, rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);
            }
        }

        attackTimer = 0f;
    }
}

