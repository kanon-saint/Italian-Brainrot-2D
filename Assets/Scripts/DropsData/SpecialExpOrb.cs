using UnityEngine;

public class SpecialExpOrb : ExpOrb
{
    public float attractionRadius = 10f;

    public override void Collect()
    {
        // Find all ExpOrbs in range and attract them
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ExpOrb orb) && orb != this)
            {
                orb.AttractToPlayer();
            }
        }

        base.Collect(); // Collect itself
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
