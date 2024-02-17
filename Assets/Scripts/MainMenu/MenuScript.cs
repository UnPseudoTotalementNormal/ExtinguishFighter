using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static MenuScript Instance;

    public enum SUBMENU
    {
        MAIN,
        GAMEMODES,
        LEVELSELECT,
    }

    public SUBMENU CurrentMenu = SUBMENU.MAIN;

    [SerializeField] private List<Transform> _subMenuCameraTransform = new List<Transform>();

    private Transform _cam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        _cam = Camera.main.transform;
    }

    private void Update()
    {
        if ((int)CurrentMenu < _subMenuCameraTransform.Count)
        {
            _cam.position = Vector3.Lerp(_cam.position, _subMenuCameraTransform[(int)CurrentMenu].position, 4 * Time.deltaTime);
            _cam.rotation = Quaternion.Lerp(_cam.rotation, _subMenuCameraTransform[(int)CurrentMenu].rotation, 4 * Time.deltaTime);
        }
    }

    public void GoToGameModeMenu()
    {
        CurrentMenu = SUBMENU.GAMEMODES;
    }

    public void GoToMainMenu() 
    {
        CurrentMenu = SUBMENU.MAIN;
    }

    public void GoToLevelSelect()
    {
        CurrentMenu = SUBMENU.LEVELSELECT;
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
