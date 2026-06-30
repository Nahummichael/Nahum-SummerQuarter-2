using UnityEngine;

public class PopUpWall : MonoBehaviour
{
   [Header("Movement Settings")]
    private float popUpHeight = 4.9f; // How high the wall pops up
    private float speed = 5f;       // How fast it moves

    private Vector3 startPos;

    void Start()
    {
        // Record the initial lowered position of the wall
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the smooth up-and-down motion using Time.time and Mathf.PingPong
        float newY = startPos.y + Mathf.PingPong(Time.time * speed, popUpHeight);
        
        // Apply the new position to the wall
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
