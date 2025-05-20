using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [SerializeField] public int health = 10;

    void Start()
    {

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
