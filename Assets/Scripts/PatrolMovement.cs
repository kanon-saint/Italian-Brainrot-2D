using UnityEngine;

public class MultiPointPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3[] patrolPositions; // Set in Inspector

    private int currentTargetIndex = 0;
    private Vector3 targetPosition;
    private bool patrolComplete = false;

    void Start()
    {
        if (patrolPositions.Length == 0)
        {
            Debug.LogWarning("No patrol positions assigned.");
            enabled = false;
            return;
        }

        targetPosition = patrolPositions[currentTargetIndex];
    }

    void Update()
    {
        if (patrolComplete) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Always play running animation
        animator.SetFloat("Speed", 1f);

        // Check if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            currentTargetIndex++;

            if (currentTargetIndex >= patrolPositions.Length)
            {
                patrolComplete = true;
            }
            else
            {
                targetPosition = patrolPositions[currentTargetIndex];
            }
        }
    }
}
