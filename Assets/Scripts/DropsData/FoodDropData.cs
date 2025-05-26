using UnityEngine;

[CreateAssetMenu(fileName = "FoodDropData", menuName = "Foods/FoodDropData")]
public class FoodDropData : ScriptableObject
{
    public GameObject prefab;
    public int healValue;
    [Range(0f, 1f)]
    public float dropChance;

}
