using UnityEngine;

public class HotPotato : MonoBehaviour
{
    public float maxTimer = 10f;
    public float remainingTime = 0f;
    public GameObject hotPotatoVisual;

    public void Initialize(float remianingTimer)
    {
        // Called whenever a player gets the hot potato
        remainingTime = remianingTimer;
    }

    private void Awake()
    {
        remainingTime = maxTimer;
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("BOOOM!");
        }
    } 
}