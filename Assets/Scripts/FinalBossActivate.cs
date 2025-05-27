using UnityEngine;

public class FinalBossActivate : MonoBehaviour
{
    public float activationRange = 10f;
    private GameObject player;

    private void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= activationRange && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else if (distance > activationRange && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
