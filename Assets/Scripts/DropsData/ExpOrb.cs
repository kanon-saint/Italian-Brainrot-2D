using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expValue = 10;
    public float pickupRadius = 3f;
    public float moveSpeed = 5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= pickupRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 0.2f)
            {
                CharacterHUD.Instance?.AddExperience(expValue);
                Destroy(gameObject);
            }
        }
    }
}
