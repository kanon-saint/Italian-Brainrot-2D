using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearManager : MonoBehaviour
{
    public static StageClearManager Instance;

    [SerializeField] private GameObject stageClearedPanel;
    [SerializeField] private float delayBeforeNextScene = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (stageClearedPanel != null)
            stageClearedPanel.SetActive(false);
    }

    public void TriggerStageClear()
    {
        if (stageClearedPanel != null)
        {
            stageClearedPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        // Reset weapon levels
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.ResetAllWeaponLevels();
        }

        StartCoroutine(ProceedToNextSceneAfterDelay());
    }

    private IEnumerator ProceedToNextSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayBeforeNextScene);

        // ✅ Save current score using ScoreManager
        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetScore();
            PlayerPrefs.SetInt("Score", currentScore);
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes. Game complete!");
        }
    }
}
