using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        // Press ESC to return to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu"); // Make sure "MainMenu" is the exact name of your scene
        }
    }
}
