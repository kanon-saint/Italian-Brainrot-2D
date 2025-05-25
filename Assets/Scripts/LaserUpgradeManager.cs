using UnityEngine;

public class LaserUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;

    private bool laserAttached = false;

    public void AttachLaserToPlayer()
    {
        if (laserAttached) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && laserPrefab != null)
        {
            GameObject laser = Instantiate(laserPrefab, player.transform);
            laser.transform.localPosition = Vector3.zero;
            laserAttached = true;
        }
        else
        {
            Debug.LogWarning("Player or laser prefab is missing.");
        }
    }
}
