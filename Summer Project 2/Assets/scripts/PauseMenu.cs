using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    
     private InputAction pauseAction;

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }


    


    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            Debug.Log("Escape key pressed");
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        UIManager.Instance.TogglePauseMenuUI(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        UIManager.Instance.TogglePauseMenuUI(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void MakeMenu()
    {
        Debug.Log("Returning to main menu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}