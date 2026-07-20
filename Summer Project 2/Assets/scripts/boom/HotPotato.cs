using UnityEngine;
using System.Collections;

public class HotPotato : MonoBehaviour 
{ 
    [Header("Timer Settings")] 
    [SerializeField] private float countdownTime = 5f; 

    [Header("Explosion Settings")] 
    [SerializeField] private GameObject explosionEffect; 

    [SerializeField] private Transform currentPlayer;

public Transform CurrentPlayer
{
    get { return currentPlayer; }
}

    private bool isExploding = false; 
    [SerializeField] private float timer; 
    private bool isPickedUp = false; 

    void Start() 
    { 
        timer = countdownTime; 
    } 


    void Update() 
    { 
        if (timer > 0 ) 
        { 
            timer -= Time.deltaTime; 
            // Update the UI timer display
            UIManager.Instance.UpdateTimer(timer);
        } 
        else
        {
            Explode(); 
        }
    }

    private void OnTriggerEnter(Collider other) 
    { 
        // Don't pick up again if someone already has it!
        if (isPickedUp) return;

        // Find all active GameObjects tagged as "Player" 
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); 

        // Safety check
        if (players.Length == 0) 
        { 
            Debug.LogWarning("No players found in the scene!"); 
            return; 
        } 

        // Pick a random player
        int randomIndex = Random.Range(0, players.Length); 
        GameObject selectedPlayer = players[randomIndex]; 

        Debug.Log($" Chosen one: {selectedPlayer.name}"); 

        // Pass the potato to the chosen player!
        PickUp(selectedPlayer.transform);
    } 

    public void PickUp(Transform playerTransform) 
    { 
        currentPlayer = playerTransform;

    transform.SetParent(playerTransform);
    transform.localPosition = new Vector3(0, 1f, 0.5f);

    Debug.Log("Potato is now held by: " + currentPlayer.name);
    } 

    void Explode() 
    { 
        if (explosionEffect != null) 
        { 
            Instantiate(explosionEffect, transform.position, transform.rotation); 
        } 
        Destroy(gameObject);


        // Try to get the PlayerController component from the current player
        Player1Controller player1Controller = currentPlayer.GetComponent<Player1Controller>();
        Player2Controller player2Controller = currentPlayer.GetComponent<Player2Controller>();

        if (player1Controller != null)
        {
            // Player 1 has the potato, so trigger their death UI
            UIManager.Instance.ShowPlayer1Death();
            GameManager.Instance.GameOver(false);
            Debug.Log("Player 1 blew up!");
        }
        else if (player2Controller != null)
        {
            // Player 2 has the potato, so trigger their death UI
            UIManager.Instance.ShowPlayer2Death();
            GameManager.Instance.GameOver(true);
            Debug.Log("Player 2 blew up!");
        }
        else
        {
            Debug.LogWarning("Current player does not have a recognized PlayerController component.");
        }


    } 

    public void ForcePickUp(Transform randomPlayer) 
    { 
        if (!isPickedUp) 
        { 
            PickUp(randomPlayer); 
        } 
    } 

    public void SetRemainingTime(float time) 
    { 
        timer = time; 
    }

    public float GetRemainingTime() 
    { 
        return timer; 
    }

    public void PassToPlayer(Transform newPlayer)
    {
    currentPlayer = newPlayer;

    transform.SetParent(newPlayer);
    transform.localPosition = new Vector3(0, 1f, 0.5f);

    Debug.Log("Potato passed to: " + newPlayer.name);
    }
}