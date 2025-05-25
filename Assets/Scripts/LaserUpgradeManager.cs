using UnityEngine;

public class LaserUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;

    private bool laserAttached = false;

    // Call this method to attach the laser to the player
    public void AttachLaserToPlayer()
    {
        if (laserAttached) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && laserPrefab != null)
        {
            GameObject laser = Instantiate(laserPrefab, player.transform);

            // Offset by 11 units on the X-axis
            laser.transform.localPosition = new Vector3(-11f, 0f, 0f);

            laserAttached = true;
        }
        if (player == null)
        {
            Debug.LogWarning("Player not found.");
        }
        else
        {
            Debug.LogWarning("Laser prefab is missing.");
        }
    }
}
