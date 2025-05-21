using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;

    public void ShowMenu()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // Works only for built build
    }
}
