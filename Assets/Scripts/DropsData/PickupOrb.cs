using UnityEngine;

public class PickupOrb : MonoBehaviour
{
    public ExpDropData expData;      // Assign if this is an EXP orb
    public FoodDropData foodData;    // Assign if this is a food orb

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
        var character = GameManager.Instance?.selectedCharacter;

        if (character != null)
        {
            // Add experience
            if (expData != null)
            {
                CharacterHUD.Instance?.AddExperience(expData.expValue);
            }

            // Heal using foodData's healValue
            else if (foodData != null)
            {
                character.Heal(foodData.healValue);
                CharacterHUD.Instance?.UpdateHP(character.currentHP);
            }
        }

        Destroy(gameObject);
    }

    public void AttractToPlayer()
    {
        isAttracted = true;
    }
}
