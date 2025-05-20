using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(2f, 4f);

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = Random.Range(0, enemies.Length);
            GameObject enemyToSpawn = enemies[randomIndex];

            Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
            spawnPosition.z = -1f; // Ensure enemy spawns in front

            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
