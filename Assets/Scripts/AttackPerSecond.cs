using UnityEngine;
using System.Collections.Generic;

public class AttackPerSecond : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int baseDamagePerSecond = 2;
    [SerializeField] private float baseDamageInterval = 1f;
    
    private int currentDamagePerSecond;
    private float currentDamageInterval;
    private float _timer;
    private HashSet<Collider2D> _enemiesInRange = new HashSet<Collider2D>();

    private void Start()
    {
        ApplyWeaponLevelBehavior();
        _timer = currentDamageInterval;
    }

    private void ApplyWeaponLevelBehavior()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("WeaponData is not assigned to AttackPerSecond.");
            currentDamagePerSecond = baseDamagePerSecond;
            currentDamageInterval = baseDamageInterval;
            return;
        }

        int level = weaponData.level;

        // More controlled damage scaling
        switch (level)
        {
            case 1:
                currentDamagePerSecond = baseDamagePerSecond;
                break;
            case 2:
                currentDamagePerSecond = baseDamagePerSecond * 2; // 20
                break;
            case 3:
                currentDamagePerSecond = baseDamagePerSecond * 3; // 30
                break;
            case 4:
                currentDamagePerSecond = baseDamagePerSecond * 4; // 40
                break;
            case 5:
                currentDamagePerSecond = baseDamagePerSecond * 5; // 50
                break;
            default:
                if (level > 5)
                {
                    int extraLevels = level - 5;
                    currentDamagePerSecond = baseDamagePerSecond * 5 + (extraLevels * 2);
                }
                break;
            // Add more levels if needed
        }

        // Optional: Slightly improve attack speed with levels
        currentDamageInterval = Mathf.Max(0.3f, baseDamageInterval * (1f - (level * 0.1f)));

        Debug.Log($"AttackPerSecond updated - Level: {level}, Damage: {currentDamagePerSecond}, Interval: {currentDamageInterval}");
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0f)
        {
            var enemiesCopy = new List<Collider2D>(_enemiesInRange);
            
            foreach (var enemyCollider in enemiesCopy)
            {
                if (enemyCollider != null && enemyCollider.CompareTag("Enemy"))
                {
                    CharacterAttributes enemyAttributes = enemyCollider.GetComponent<CharacterAttributes>();
                    if (enemyAttributes != null)
                    {
                        enemyAttributes.TakeDamage(currentDamagePerSecond);
                    }
                }
            }
            _timer = currentDamageInterval;
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