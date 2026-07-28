using UnityEngine;

public class HotPotato : MonoBehaviour
{
    public float maxTimer = 10f;
    public float remainingTime = 0f;
    public PotatoVisual hotPotatoVisual;

    public void Initialize(float remianingTimer)
    {
        // Called whenever a player gets the hot potato
        remainingTime = remianingTimer;
    }

    private void Awake()
    {
        // set the time of the potato
        remainingTime = maxTimer;
        // initialize the potato visual
        hotPotatoVisual = GetComponentInChildren<PotatoVisual>(true);
        // disable the potato visual
        TogglePotatoVisual(true);
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            // update the UI with the remaining time
            UIManager.Instance.UpdateTimer(remainingTime);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("BOOOM!");
            // tell the UI which player died based on who's holding this potato
            if (GetComponent<Player1Controller>() != null)
            {
                UIManager.Instance.ShowPlayer1Death();
                
            }
            else if (GetComponent<Player2Controller>() != null)
            {
                UIManager.Instance.ShowPlayer2Death();
            }

            // figure out who was holding the potato when it exploded
            bool player1Held = GetComponent<Player1Controller>() != null;

            // gameManager handles the scoring
            // player1Held == true means Player1 lost, so Player2 gets the point.
            GameManager.Instance.GameOver(!player1Held);
            //show the game over panel
            UIManager.Instance.ToggleGameOverUI(true);
        }
    } 

    private void TogglePotatoVisual(bool flag)
    {
        // turn the potato visual on or off based on the given flag
        hotPotatoVisual.gameObject.SetActive(flag);
    }

    private void OnDestroy()
    {
        // turn off the potato visual
        TogglePotatoVisual(false);
    }
}