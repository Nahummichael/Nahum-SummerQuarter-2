using UnityEngine;

public class PassPotato : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyCode passButton = KeyCode.Space;

    void Update()
    {
        // Check if this specific player presses the pass button
        if (Input.GetKeyDown(passButton))
        {
            // Look for a HotPotato child object attached to this player
            HotPotato heldPotato = GetComponentInChildren<HotPotato>();

            // If this player is holding the potato, pass it!
            if (heldPotato != null)
            {
                heldPotato.PassToRandomPlayer();
            }
        }
    }
}
