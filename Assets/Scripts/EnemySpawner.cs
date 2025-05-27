using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimedEnemySpawn
{
    public float triggerTime;             // One-time spawn at this time
    public GameObject enemyPrefab;
}

[System.Serializable]
public class TimedEnemyRange
{
    public float startTime;              // Time range start
    public float endTime;                // Time range end
    public GameObject[] enemyPrefabs;    // Enemies allowed to spawn during this time
}

public class EnemySpawner : MonoBehaviour
{
    [Header("General Spawn Settings")]
    [SerializeField] private GameObject[] defaultEnemies;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(2f, 4f);
    [SerializeField] private float spawnAccelerationRate = 0.95f;
    [SerializeField] private float accelerationInterval = 30f;

    [Header("Timed One-Time Spawns")]
    [SerializeField] private List<TimedEnemySpawn> timedSpawns;

    [Header("Timed Spawn Ranges")]
    [SerializeField] private List<TimedEnemyRange> spawnRanges;

    private float elapsedTime = 0f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(AccelerateSpawnRate());
        StartCoroutine(HandleTimedSpawns());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            GameObject enemyToSpawn = ChooseEnemyForCurrentTime();

            if (enemyToSpawn != null)
            {
                Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
                spawnPosition.z = -1f;
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
        }
    }

    private GameObject ChooseEnemyForCurrentTime()
    {
        List<GameObject> eligibleEnemies = new List<GameObject>(defaultEnemies);

        foreach (var range in spawnRanges)
        {
            if (elapsedTime >= range.startTime && elapsedTime <= range.endTime)
            {
                eligibleEnemies.AddRange(range.enemyPrefabs);
            }
        }

        if (eligibleEnemies.Count == 0)
            return null;

        int index = Random.Range(0, eligibleEnemies.Count);
        return eligibleEnemies[index];
    }

    private IEnumerator AccelerateSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(accelerationInterval);

            spawnIntervalRange.x *= spawnAccelerationRate;
            spawnIntervalRange.y *= spawnAccelerationRate;

            spawnIntervalRange.x = Mathf.Max(0.5f, spawnIntervalRange.x);
            spawnIntervalRange.y = Mathf.Max(1f, spawnIntervalRange.y);
        }
    }

    private IEnumerator HandleTimedSpawns()
    {
        foreach (var timed in timedSpawns)
        {
            float wait = Mathf.Max(0, timed.triggerTime - elapsedTime);
            yield return new WaitForSeconds(wait);

            if (timed.enemyPrefab != null)
            {
                Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
                spawnPosition.z = -1f;
                Instantiate(timed.enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
