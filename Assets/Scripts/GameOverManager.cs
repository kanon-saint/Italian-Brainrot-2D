using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

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
            Time.timeScale = 0f;

            int currentScore = ScoreManager.Instance?.GetScore() ?? 0;
            int highScore = PlayerPrefs.GetInt("HighScore", 0);

            if (currentScore > highScore)
            {
                PlayerPrefs.SetInt("HighScore", currentScore);
                PlayerPrefs.Save();
                finalScoreText.text = $"New High Score!\nScore: {currentScore}";
            }
            else
            {
                finalScoreText.text = $"Score: {currentScore}";
            }
        }
        else
        {
            Debug.LogError("GameOverPanel not assigned in inspector!");
        }
    }



    public void Retry()
    {
        Time.timeScale = 1f;
        ResetWeapons();

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToCharacterSelection()
    {
        Time.timeScale = 1f;
        ResetWeapons();

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene("CharacterSelection");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        ResetWeapons();

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void ResetWeapons()
    {
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.ResetAllWeaponLevels();
        }
        else
        {
            Debug.LogWarning("WeaponManager instance not found!");
        }
    }

}
