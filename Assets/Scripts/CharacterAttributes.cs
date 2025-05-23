using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [SerializeField] public int health = 10;

    private bool isDead = false; // Prevent multiple calls

    void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;

            // Call drop BEFORE destroying the object
            GetComponent<EnemyDrops>()?.DropExp();

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
