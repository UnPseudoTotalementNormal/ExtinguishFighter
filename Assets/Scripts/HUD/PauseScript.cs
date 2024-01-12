using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static PauseScript Instance;

    [SerializeField] private GameObject _pauseOwnerObject;

    private void Awake()
    {
        Instance = this;
        UnPauseGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseOwnerObject.activeSelf)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (_pauseOwnerObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseOwnerObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        _pauseOwnerObject.SetActive(false);

        if (PlayerController.Instance) 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
