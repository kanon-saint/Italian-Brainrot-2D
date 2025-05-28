using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public CharacterHUD hud;

    private void Awake()
    {
        // Reference the WeaponManager function
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.ResetAllWeaponLevels();
            WeaponManager.Instance.DisplayEquippedWeapons();
        }
        else
        {
            Debug.LogWarning("WeaponManager instance not found. Cannot reset weapon levels.");
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            CharacterData selectedData = GameManager.Instance.selectedCharacter;

            selectedData.ResetHP();
            selectedData.Heal(selectedData.maxHP);

            GameObject characterInstance = Instantiate(selectedData.characterPrefab, spawnPoint.position, Quaternion.identity);

            if (hud != null)
            {
                hud.InitializeHUD(selectedData.characterImage, selectedData.maxHP, selectedData.maxHP);
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
