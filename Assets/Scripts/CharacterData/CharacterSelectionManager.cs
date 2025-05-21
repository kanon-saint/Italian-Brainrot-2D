using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    public CharacterData SelectedCharacter { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public void SelectCharacter(CharacterData character)
    {
        SelectedCharacter = character;
        Debug.Log("Selected character: " + character.characterName);
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(1f); // Optional delay
        SceneManager.LoadScene("SampleScene");
    }
}
