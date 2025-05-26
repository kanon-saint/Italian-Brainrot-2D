using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmationPanel;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // --- Settings ---
    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void SaveSettings()
    {
        // Add your settings save logic here
        Debug.Log("Settings saved.");
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void CancelSettings()
    {
        // Optionally reset settings changes here
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    // --- Quit Confirmation ---
    public void ShowQuitConfirmation()
    {
        pauseMenuPanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    public void ConfirmQuitToMainMenu()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance?.selectedCharacter != null)
        {
            GameManager.Instance.selectedCharacter.ResetHP();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelQuit()
    {
        confirmationPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}
