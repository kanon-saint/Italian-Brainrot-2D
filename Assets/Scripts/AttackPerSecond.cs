using UnityEngine;
using System.Collections.Generic;

public class AttackPerSecond : MonoBehaviour
{
    [SerializeField] public int damagePerSecond = 10;
    [SerializeField] public float damageInterval = 1f; // Time between damage ticks in seconds
    
    private float _timer;
    private HashSet<Collider2D> _enemiesInRange = new HashSet<Collider2D>();

    private void Update()
    {
        // Count down the timer
        _timer -= Time.deltaTime;
        
        // When timer reaches zero, deal damage to all enemies in range
        if (_timer <= 0f)
        {
            foreach (var enemyCollider in _enemiesInRange)
            {
                if (enemyCollider != null && enemyCollider.CompareTag("Enemy"))
                {
                    CharacterAttributes enemyAttributes = enemyCollider.GetComponent<CharacterAttributes>();
                    if (enemyAttributes != null)
                    {
                        enemyAttributes.TakeDamage(damagePerSecond);
                        Debug.Log($"Enemy hit! Remaining HP: {enemyAttributes.health}");
                    }
                }
            }
            _timer = damageInterval; // Reset the timer
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemiesInRange.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemiesInRange.Remove(other);
        }
    }
}