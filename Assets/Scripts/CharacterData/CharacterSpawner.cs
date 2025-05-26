using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    public CharacterHUD hud;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            CharacterData selectedData = GameManager.Instance.selectedCharacter;

            // Instantiate the character prefab
            GameObject characterInstance = Instantiate(selectedData.characterPrefab, spawnPoint.position, Quaternion.identity);

            // Try to get a health component if you have one (optional)
            int currentHP = selectedData.maxHP; // Default to max HP
            int maxHP = selectedData.maxHP;

            // Initialize HUD
            if (hud != null)
            {
                hud.InitializeHUD(selectedData.characterImage, currentHP, maxHP);
            }
            else
            {
                Debug.LogWarning("CharacterHUD reference not set on CharacterSpawner.");
            }
        }
        else
        {
            Debug.LogError("No selected character found in GameManager.");
        }
    }
}
