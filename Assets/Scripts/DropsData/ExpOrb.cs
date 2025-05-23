using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public ExpDropData data; // Assign in prefab
    public float pickupRadius = 3f;
    public float moveSpeed = 5f;

    protected Transform player;
    protected bool isAttracted = false;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Attracted manually or within pickup radius
        if (isAttracted || distance <= pickupRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance <= 0.2f)
            {
                Collect();
            }
        }
    }

    public virtual void Collect()
    {
        if (data != null)
        {
            CharacterHUD.Instance?.AddExperience(data.expValue);
        }
        Destroy(gameObject);
    }

    public void AttractToPlayer()
    {
        isAttracted = true;
    }
}
