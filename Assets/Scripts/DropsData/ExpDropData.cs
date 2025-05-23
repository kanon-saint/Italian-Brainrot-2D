using UnityEngine;

[CreateAssetMenu(fileName = "NewExpDrop", menuName = "EXP/Drop")]
public class ExpDropData : ScriptableObject
{
    public GameObject prefab;
    public int expValue;
    [Range(0f, 1f)]
    public float dropChance; // Between 0.0 and 1.0
}
