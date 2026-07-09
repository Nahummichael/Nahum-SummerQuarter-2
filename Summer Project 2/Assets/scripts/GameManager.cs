using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // this will be a singleton instance
    public static GameManager Instance { get; private set;}
    [SerializeField] public static bool isGameOver = false; // a flag to determine if the game is over or not

    
    private void Awake()
    {
        // check the singleton
        if (Instance == null)
        {
            // assign this instance of the script as the main instance
            Instance = this;
        }
        else // if theres already a GameManager assigned
        {
            // destroys any extra copy of this script
            Destroy(gameObject);
        }

        // reset the game over flag
        isGameOver = false;

    }

    // take the player back to the main menu when they win or lose
    public void GameOver()
    {
        if (isGameOver) return; // if the game is already over, do nothing
        // set the game to be over
        isGameOver = true;
        // trigger the over UI
         // trigger the game over UI the the game ends
        UIManager.Instance.ToggleGameOverUI(true);
    }


    public void LoadMainMenu()
    {
        // load the main menu scene
        SceneManager.LoadScene("0");
    }

    public void LoadCurrentScene()
    {
        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
