using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("EXP Drops")]
    [SerializeField] private List<ExpDropData> expDrops;
    [SerializeField] private int numberOfExpDrops = 1;

    [Header("Food Drops")]
    [SerializeField] private List<FoodDropData> foodDrops;
    [SerializeField] private float foodDropChance = 0.01f;

    public void DropItems()
    {
        DropExp();
        TryDropFood();
    }

    private void DropExp()
    {
        for (int i = 0; i < numberOfExpDrops; i++)
        {
            ExpDropData selected = GetRandomExpDrop();
            if (selected != null)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                Instantiate(selected.prefab, transform.position + spawnOffset, Quaternion.identity);
            }
        }
    }

    private void TryDropFood()
    {
        if (Random.value <= foodDropChance)
        {
            FoodDropData selected = GetRandomFoodDrop();
            if (selected != null)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
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

    private FoodDropData GetRandomFoodDrop()
    {
        float totalChance = 0f;
        foreach (var drop in foodDrops)
            totalChance += drop.dropChance;

        float randomValue = Random.value * totalChance;
        float cumulative = 0f;

        foreach (var drop in foodDrops)
        {
            cumulative += drop.dropChance;
            if (randomValue <= cumulative)
                return drop;
        }

        return null;
    }
}
