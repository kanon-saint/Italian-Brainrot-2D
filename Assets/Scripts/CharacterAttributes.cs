using System.Collections;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [Header("Health Scaling")]
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float healthScalingFactor = 0.5f; // Extra HP per second

    [Header("Score")]
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private bool isBoss = false;

    [Header("References")]
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hurtSound;

    private int health;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Scale health based on time since level started
        float elapsedTime = Time.time;
        health = Mathf.RoundToInt(baseHealth + (elapsedTime * healthScalingFactor));
    }

    private void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;

            // Drop items
            GetComponent<EnemyDrops>()?.DropItems();

            // Add score
            ScoreManager.Instance?.AddScore(scoreValue);

            if (isBoss)
            {
                StageClearManager.Instance?.TriggerStageClear();
            }

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(PlayHurtThenWalk());

        StartCoroutine(PlayHurtSfx());
    }

    private IEnumerator PlayHurtThenWalk()
    {
        animator.Play("Hurt");

        // Wait for the hurt animation to finish (adjust to match your actual clip length)
        yield return new WaitForSeconds(0.3f);

        animator.Play("Walk");
    }

    private IEnumerator PlayHurtSfx()
    {
        audioSource.PlayOneShot(hurtSound);
        yield return new WaitForSeconds(0.3f);
    }
}
