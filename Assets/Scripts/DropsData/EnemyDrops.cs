using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] private List<ExpDropData> expDrops;
    [SerializeField] private int numberOfDrops = 1;

    public void DropExp()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            ExpDropData selected = GetRandomExpDrop();
            if (selected != null)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                Instantiate(selected.prefab, transform.position + spawnOffset, Quaternion.identity);
            }
        }
    }

    private ExpDropData GetRandomExpDrop()
    {
        float totalChance = 0f;
        foreach (var drop in expDrops)
            totalChance += drop.dropChance;

        float randomValue = Random.value * totalChance;
        float cumulative = 0f;

        foreach (var drop in expDrops)
        {
            cumulative += drop.dropChance;
            if (randomValue <= cumulative)
                return drop;
        }

        return null;
    }
}
