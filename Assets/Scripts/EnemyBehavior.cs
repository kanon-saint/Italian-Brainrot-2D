using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Animator animator;

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
        animator.SetFloat("Speed", move.magnitude / Time.deltaTime);

        if (player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face right
        }
    }
}
