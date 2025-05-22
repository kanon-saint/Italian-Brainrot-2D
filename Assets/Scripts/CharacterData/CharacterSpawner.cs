using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            Instantiate(GameManager.Instance.selectedCharacter.characterPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No selected character found in GameManager.");
        }
    }
}
