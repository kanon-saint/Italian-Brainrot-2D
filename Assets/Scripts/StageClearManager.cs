using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearManager : MonoBehaviour
{
    public static StageClearManager Instance;

    [SerializeField] private GameObject stageClearedPanel;
    [SerializeField] private float delayBeforeNextScene = 3f; // Optional delay

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

        // Proceed to next scene after a short delay
        StartCoroutine(ProceedToNextSceneAfterDelay());
    }

    private IEnumerator ProceedToNextSceneAfterDelay()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(delayBeforeNextScene);

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
