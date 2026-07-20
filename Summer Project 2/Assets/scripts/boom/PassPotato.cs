using UnityEngine;


public class PassPotato : MonoBehaviour 
{ 
    // The player currently holding the potato
    private GameObject currentHolder;

    void Update()
    {
        // Follow the current holder
        if (currentHolder != null)
        {
            transform.position = currentHolder.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we collide with a player, pass the potato to them
        if (collision.gameObject.CompareTag("Player"))
        {
            currentHolder = collision.gameObject;
        }
    }


}
