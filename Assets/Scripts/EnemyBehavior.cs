using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        transform.position += move;

        // if (move != Vector3.zero)
        // {
        //     transform.forward = direction;
        // }
    }
}
