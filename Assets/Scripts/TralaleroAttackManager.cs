using UnityEngine;

public class TralaleroAttackManager : MonoBehaviour
{
    [SerializeField] private GameObject defaultAttackPfx;
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
            TralaleroAttack();
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

    private void TralaleroAttack()
    {
        attacking = true;

        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Determine direction: left or right
        bool isRight = mousePos.x >= transform.position.x;

        // Set rotation to either right (0°) or left (180°)
        float angle = isRight ? 0f : 180f;

        attackArea.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        attackArea.SetActive(true);

        if (defaultAttackPfx != null)
        {
            Vector3 originalEuler = defaultAttackPfx.transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(angle, originalEuler.y, originalEuler.z);

            currentAttackPfx = Instantiate(defaultAttackPfx, attackArea.transform.position, rotation);
        }
    }
}
