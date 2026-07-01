using UnityEngine;

public class HotPotato : MonoBehaviour 
{ 
    [Header("Timer Settings")] 
    [SerializeField] private float countdownTime = 5f; 

    [Header("Explosion Settings")] 
    [SerializeField] private GameObject explosionEffect; 

    private bool isExploding = false; 
    private float timer; 
    private bool isPickedUp = false; 

    void Start() 
    { 
        timer = countdownTime; 
    } 

    void Update() 
    { 
        if (timer > 0) 
        { 
            timer -= Time.deltaTime; 
        } 
        else if (!isExploding) 
        { 
            Explode(); 
        } 
    } 

    private void OnTriggerEnter(Collider other) 
    { 
        // 🛑 Don't pick up again if someone already has it!
        if (isPickedUp) return;

        // 🏷️ Find all active GameObjects tagged as "Player" 
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); 

        // ❌ Safety check
        if (players.Length == 0) 
        { 
            Debug.LogWarning("No players found in the scene!"); 
            return; 
        } 

        // 🎲 Pick a random player
        int randomIndex = Random.Range(0, players.Length); 
        GameObject selectedPlayer = players[randomIndex]; 

        Debug.Log($"🎉 Chosen one: {selectedPlayer.name}"); 

        // 🎯 FIX: Pass the potato to the chosen player!
        PickUp(selectedPlayer.transform);
    } 

    void PickUp(Transform playerTransform) 
    { 
        isPickedUp = true; 

        // 1. Disable collider so it stops triggering
        GetComponent<Collider>().enabled = false; 

        // 2. Snap it to the player 
        transform.SetParent(playerTransform); 
        transform.localPosition = new Vector3(0, 1f, 0.5f); 
    } 

    void Explode() 
    { 
        isExploding = true; 
        if (explosionEffect != null) 
        { 
            Instantiate(explosionEffect, transform.position, transform.rotation); 
        } 
        Destroy(gameObject); 
    } 

    public void ForcePickUp(Transform randomPlayer) 
    { 
        if (!isPickedUp) 
        { 
            PickUp(randomPlayer); 
        } 
    } 
}
