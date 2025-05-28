using UnityEngine;
using UnityEngine.SceneManagement;  // <-- Add this

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CharacterData selectedCharacter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Call DisplayEquippedWeapons if WeaponManager instance is ready
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.DisplayEquippedWeapons();
        }
        else
        {
            Debug.LogWarning("WeaponManager instance not found on scene load.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
