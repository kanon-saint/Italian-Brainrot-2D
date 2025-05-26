using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        // Check if no character selected at start, then game over immediately
        if (GameManager.Instance?.selectedCharacter == null)
        {
            Debug.LogWarning("No character selected! Triggering game over.");
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;  // pause the game
        }
        else
        {
            Debug.LogError("GameOverPanel not assigned in inspector!");
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToCharacterSelection()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene("CharacterSelection");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene("MainMenu");
    }
}
