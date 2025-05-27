using UnityEngine;

public class Plus2Damage : MonoBehaviour
{
    [SerializeField] public int attackDamage;
    [SerializeField] private WeaponData weaponData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                int finalDamage = attackDamage;

                if (weaponData != null && weaponData.level > 5)
                {
                    int extraDamage = (weaponData.level - 5) * 2;
                    finalDamage += extraDamage;
                }

                Debug.Log($"Bullet hit enemy. Weapon level: {weaponData?.level}, Final Damage: {finalDamage}");

                enemyAttributes.TakeDamage(finalDamage);
            }
        }
    }
}
