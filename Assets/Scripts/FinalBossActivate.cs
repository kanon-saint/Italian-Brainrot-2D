using UnityEngine;

public class FinalBossActivate : MonoBehaviour
{
    public float activationRange = 10f;
    private GameObject player;
    [SerializeField] private MonoBehaviour scriptToToggle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }

        if (scriptToToggle == null)
        {
            Debug.LogWarning("No script assigned to toggle!");
        }
    }

    private void Update()
    {
        if (player == null || scriptToToggle == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        scriptToToggle.enabled = distance <= activationRange;
    }
}
