using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float healthScalingFactor = 0.5f; // HP per second

    private int health;
    private bool isDead = false;

    private void Start()
    {
        // Scale health based on elapsed time since level started
        float elapsedTime = Time.time;
        health = Mathf.RoundToInt(baseHealth + (elapsedTime * healthScalingFactor));
    }

    private void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;

            GetComponent<EnemyDrops>()?.DropItems();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
