using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] public int attackDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterAttributes enemyAttributes = other.GetComponent<CharacterAttributes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(attackDamage);
                Debug.Log("Enemy hit! Remaining HP: " + enemyAttributes.health);
            }
        }
    }
}
