using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private WeaponData weaponData;

    [Header("Damage Settings")]
    [SerializeField] private int baseDamage;
    [SerializeField] private int damagePerLevel = 1;

    // Dynamic damage calculation based on weapon level
    public int AttackDamage
    {
        get
        {
            if (weaponData == null) return baseDamage;
            int level = Mathf.Max(1, weaponData.level);
            return baseDamage + (level - 1) * damagePerLevel;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(AttackDamage);
                Debug.Log($"Attacked enemy with damage: {AttackDamage}");
            }
        }
    }
}
