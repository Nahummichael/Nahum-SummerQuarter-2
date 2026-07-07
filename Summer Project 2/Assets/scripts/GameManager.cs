using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // this will be a singleton instance
    public static GameManager Instance { get; private set; }

    
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
    }



    // take the player back to the main menu when they win or lose
    public void GameOver()
    {
        // go to main menu
        SceneManager.LoadScene(0);
    }
}
