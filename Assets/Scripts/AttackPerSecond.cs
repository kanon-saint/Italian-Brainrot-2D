using UnityEngine;
using System.Collections.Generic;

public class AttackPerSecond : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    [Header("Base Stats")]
    [SerializeField] private int baseDamagePerSecond = 4;
    [SerializeField] private float baseDamageInterval = 1f;

    [Header("Level Scaling")]
    [SerializeField] private int damagePerLevel = 4; // Linear scaling like LaserTrigger
    [SerializeField] private float intervalReductionPerLevel = 0.1f;

    private float _timer;
    private HashSet<Collider2D> _enemiesInRange = new HashSet<Collider2D>();

    // Dynamic properties
    private int Level => Mathf.Max(1, weaponData != null ? weaponData.level : 1);

    private int CurrentDamagePerSecond => baseDamagePerSecond + (Level - 1) * damagePerLevel;
    private float CurrentDamageInterval => Mathf.Max(0.3f, baseDamageInterval - (Level - 1) * intervalReductionPerLevel);

    private void Start()
    {
        _timer = CurrentDamageInterval;
        Debug.Log($"[AttackPerSecond] Initialized at Level {Level} with Damage {CurrentDamagePerSecond}, Interval {CurrentDamageInterval}");
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
                        Debug.Log($"Damage applied: {CurrentDamagePerSecond} to {enemyCollider.name}");
                        enemyAttributes.TakeDamage(CurrentDamagePerSecond);
                    }
                }
            }
            _timer = CurrentDamageInterval;
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
