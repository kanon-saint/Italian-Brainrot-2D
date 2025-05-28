using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private float returnDelay = 90f; // Time before returning to MainMenu
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Make sure this matches your scene name

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(ReturnToMainMenuAfterDelay());
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    private IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ReturnToMainMenuButtonClick()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
