using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // this will be a singleton instance
    public static GameManager Instance { get; private set;}
    [SerializeField] public static bool isGameOver = false; // a flag to determine if the game is over or not
    [SerializeField] private TextMeshProUGUI player1wins;
    [SerializeField] private TextMeshProUGUI player2wins;
    // stores the number of wins for each player
    public int player1Wins {get; private set;}
    public int player2Wins {get; private set;}

    // 1. players play the game
    // 2. when the game ends calculate the number of wins between each player
    
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

        // resets the score for now
        PlayerPrefs.DeleteAll();

        player1Wins = PlayerPrefs.GetInt("player1", 0);
        player2Wins = PlayerPrefs.GetInt("player2", 0);

        Debug.Log($"Player 1 wins: {player1Wins}, Player 2 wins: {player2Wins}");
    }

    public void Update()
    {
        player1wins.text = player1Wins.ToString();
        player2wins.text = player2Wins.ToString();
    }

    // take the player back to the main menu when they win or lose
    public void GameOver(bool player1Won)
    {
        if (isGameOver) return; // if the game is already over, do nothing
        // set the game to be over
        isGameOver = true;
        // trigger the over UI
         // trigger the game over UI the the game ends
        UIManager.Instance.ToggleGameOverUI(true);

        if (player1Won)
        {
            // checking which player won
            player1Wins ++;
            PlayerPrefs.SetInt("player1", player1Wins);
            PlayerPrefs.Save();
        }
        else
        {
            player2Wins ++;
            PlayerPrefs.SetInt("player2", player2Wins);
            PlayerPrefs.Save();
        }

        // update the UI with the new scores
        // UIManager.Instance.PlayerScores(player1Wins, player2Wins);
    }

   

        public void StartRound()
    {
        Player1Controller player1 = FindObjectOfType<Player1Controller>();
        Player2Controller player2 = FindObjectOfType<Player2Controller>();

        // randomly pick who starts with it
        bool player1Starts = Random.value > 0.5f;

        if (player1Starts)
        {
            HotPotato potato = player1.gameObject.AddComponent<HotPotato>();
            player1.isHoldingPotato = true;
            player2.isHoldingPotato = false;
        }
        else
        {
            HotPotato potato = player2.gameObject.AddComponent<HotPotato>();
            player2.isHoldingPotato = true;
            player1.isHoldingPotato = false;
        }
    }


    public void LoadMainMenu()
    {
        // load the main menu scene
        SceneManager.LoadScene(0);
    }

    public void LoadCurrentScene()
    {
        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
