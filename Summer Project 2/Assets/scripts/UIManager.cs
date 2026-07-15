using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;


    [SerializeField] private GameObject player1end;
    [SerializeField] private GameObject player2end;

    // Awake is called when the game object is activated
    private void Awake()
    {
        // check the singleton
        // make this the only instance of the script when there isnt any other
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy any duplicates of this script
            Destroy(gameObject);
        }
        // turn off the game over panel on game start
        ToggleGameOverUI(false);

        // Hide both death canvases when the game starts
        player1end.SetActive(false);
        player2end.SetActive(false);
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"time: {time:F1}";
    }

    // Shows Player 1's death canvas
    public void ShowPlayer1Death()
    {
        player1end.SetActive(true);
        player2end.SetActive(false);
    }

    // Shows Player 2's death canvas
    public void ShowPlayer2Death()
    {
        player1end.SetActive(false);
        player2end.SetActive(true);
    }

    // toggles the UI on or off based on the boolean (or the true or false statement) passed into it
    public void ToggleGameOverUI(bool flag)
    {
        gameOverPanel.SetActive(flag);
    }

   
}
