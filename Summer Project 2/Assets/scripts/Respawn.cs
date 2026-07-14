using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Rigidbody rb = collision.rigidbody;
        
        if (rb == null) return;
        collision.transform.position = respawnPoint.position;
    }
}
