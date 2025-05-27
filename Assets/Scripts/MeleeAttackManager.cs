using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.25f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject defaultAttackPfx;

    [Header("Attack Area")]
    [SerializeField] private Transform attackAreaTransform;

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    private GameObject attackArea;
    private GameObject currentAttackPfx;

    private bool isAttacking = false;
    private float attackCooldownTimer = 0f;
    private float attackDurationTimer = 0f;

    private void Start()
    {
        attackArea = attackAreaTransform != null ? attackAreaTransform.gameObject : transform.GetChild(0).gameObject;
        attackArea.SetActive(false);
    }

    private void Update()
    {
        attackCooldownTimer += Time.deltaTime;

        if (attackCooldownTimer >= attackCooldown)
        {
            PerformMeleeAttack();
            attackCooldownTimer = 0f;
        }

        if (isAttacking)
        {
            attackDurationTimer += Time.deltaTime;

            if (attackDurationTimer >= attackDuration)
            {
                EndAttack();
            }
        }
    }

    private void PerformMeleeAttack()
    {
        isAttacking = true;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        attackArea.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        attackArea.SetActive(true);

        if (defaultAttackPfx != null)
        {
            Vector3 originalEuler = defaultAttackPfx.transform.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(angle - 180f, originalEuler.y, originalEuler.z);

            // Use attackArea's current position instead of recalculated position
            currentAttackPfx = Instantiate(defaultAttackPfx, attackArea.transform.position, rotation);
        }

        PlayAttackSound();
    }

    private void EndAttack()
    {
        attackArea.SetActive(false);

        if (currentAttackPfx != null)
        {
            Destroy(currentAttackPfx);
        }

        isAttacking = false;
        attackDurationTimer = 0f;
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.clip = attackSound;
            audioSource.Play();
        }
    }
}
