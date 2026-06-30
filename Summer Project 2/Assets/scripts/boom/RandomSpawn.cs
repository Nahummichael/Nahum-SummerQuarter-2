using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Setup")]
    public GameObject potatoPrefab; // Drag your Potato Prefab here

    void Start()
    {
        SpawnPotatoOnRandomPlayer();
    }

    public void SpawnPotatoOnRandomPlayer()
    {
        // 1. Find all GameObjects in the scene tagged as "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 2. Safety check: make sure there are actually players in the game
        if (players.Length == 0)
        {
            Debug.LogWarning("No players found with the tag 'Player'!");
            return;
        }

        // 3. Pick a random index from the array
        int randomIndex = Random.Range(0, players.Length);
        GameObject chosenPlayer = players[randomIndex];

        // 4. Spawn the potato at the chosen player's position
        GameObject newPotato = Instantiate(potatoPrefab, chosenPlayer.transform.position, Quaternion.identity);

        // 5. Force the potato to be instantly picked up by that player
        HotPotato potatoScript = newPotato.GetComponent<HotPotato>();
        if (potatoScript != null)
        {
            // We call a public pickup method (we will update the potato script below to allow this)
            potatoScript.ForcePickUp(chosenPlayer.transform);
        }
    }
}
