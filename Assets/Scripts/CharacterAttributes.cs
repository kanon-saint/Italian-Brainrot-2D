using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [Header("Health Scaling")]
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float healthScalingFactor = 0.5f; // Extra HP per second

    [Header("Score")]
    [SerializeField] private int scoreValue = 10;

    private int health;
    private bool isDead = false;

    private void Start()
    {
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

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
