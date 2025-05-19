using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private float attackDuration = 0.25f;
    [SerializeField] private float attackCooldown = 1f;

    private GameObject attackArea;
    private GameObject currentSlashEffect;

    private bool attacking = false;
    private float attackTimer = 0f;
    private float durationTimer = 0f;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackArea.SetActive(false);
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }

        if (attacking)
        {
            durationTimer += Time.deltaTime;

            if (durationTimer >= attackDuration)
            {
                attackArea.SetActive(false);

                if (currentSlashEffect != null)
                {
                    Destroy(currentSlashEffect);
                }

                attacking = false;
                durationTimer = 0f;
            }
        }
    }

    private void Attack()
    {
        attacking = true;

        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;

        // Position the attack area at a fixed distance in that direction
        Vector3 attackPosition = transform.position + direction * attackDistance;
        attackPosition.z = -1f; // ensure it's rendered above sprites

        // Apply rotation and position
        attackArea.transform.position = attackPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackArea.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        attackArea.SetActive(true);

        if (slashEffectPrefab != null)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            currentSlashEffect = Instantiate(slashEffectPrefab, attackPosition, rotation);
        }
    }
}
