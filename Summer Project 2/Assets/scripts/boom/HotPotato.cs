using UnityEngine;

public class HotPotato : MonoBehaviour
{
   [Header("Timer Settings")]
    [SerializeField] private float countdownTime = 5f;

    [Header("Explosion Settings")]
    private GameObject explosionEffect;

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
        // Only pick up if it hasn't been grabbed yet and the object has a "Player" tag
        if (!isPickedUp && other.CompareTag("Player"))
        {
            PickUp(other.transform);
        }
    }

    void PickUp(Transform playerTransform)
    {
        isPickedUp = true;

        // 1. Disable the collider so it doesn't trigger multiple pickups
        GetComponent<Collider>().enabled = false;

        // 2. Snap it to the player
        transform.SetParent(playerTransform);
        transform.localPosition = new Vector3(0, 1f, 0.5f); // Adjust position as needed
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


public void ForcePickUp(Transform playerTransform)
{
    if (!isPickedUp)
    {
        PickUp(playerTransform);
    }
}
}
