using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LaunchGame()
    {
        SceneManager.LoadSceneAsync("TutorialLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
