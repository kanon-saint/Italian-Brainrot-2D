using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject defaultAttackPfx;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private float attackDuration = 0.25f;
    [SerializeField] private float attackCooldown = 1f;

    private GameObject attackArea;
    private GameObject currentAttackPfx;

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

                if (currentAttackPfx != null)
                {
                    Destroy(currentAttackPfx);
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

        // Calculate rotation angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the attack area to face the mouse
        attackArea.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        attackArea.SetActive(true);

        // If slash effect prefab is set, instantiate it at attackArea's position with correct rotation
        if (defaultAttackPfx != null)
        {
            Vector3 originalEuler = defaultAttackPfx.transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(angle - 180f, originalEuler.y, originalEuler.z);

            // Use attackArea's current position instead of recalculated position
            currentAttackPfx = Instantiate(defaultAttackPfx, attackArea.transform.position, rotation);
        }
    }


}
