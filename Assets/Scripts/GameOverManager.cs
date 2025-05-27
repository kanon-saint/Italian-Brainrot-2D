using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonClickSound;

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

            PlayGameOverSound();
        }
        else
        {
            Debug.LogError("GameOverPanel not assigned in inspector!");
        }
    }


    public void Retry()
    {
        PlaySound(buttonClickSound);

        Time.timeScale = 1f;
        ResetWeapons();
        ScoreManager.Instance?.ResetScore();

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }

        SceneManager.LoadScene("ForestStage");
    }

    public void GoToCharacterSelection()
    {
        PlaySound(buttonClickSound);

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
        PlaySound(buttonClickSound);

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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.Stop(); // Stops ongoing music or SFX
            audioSource.clip = gameOverSound;
            audioSource.loop = false; // optional, depending on clip type
            audioSource.Play();
        }
    }

}
