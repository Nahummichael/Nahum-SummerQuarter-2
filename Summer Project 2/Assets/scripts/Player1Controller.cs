using UnityEngine;
using UnityEngine.InputSystem; 

public class Player1Controller : MonoBehaviour
{
    [SerializeField] private HotPotato hotPotato;
    [SerializeField] private Transform otherPlayer; // drag Player2 here in Inspector

    [SerializeField, Tooltip("A variable to store the input action sheet the player needs for inputs")] 
    private InputActionAsset InputActions;

    //THE ACTUAL ACTIONS
    private InputAction moveAction;
    private InputAction jumpAction;

    //LOGIC
    private Vector2 moveInput;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1f;


    //Components
    [SerializeField] private Rigidbody rb;
    public bool isHoldingPotato = false; // New variable to track if the player is holding the potato


    //player settings 
    [SerializeField, Tooltip("the speed the player moves at")] private float moveSpeed = 5f;
    [SerializeField, Tooltip("the player's jump height")] private float jumpForce = 5f;

    // awake is called when the instance first loads
    private void Awake()
    {
        //assigns the input action variables to their actual input action
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        //assign the rb variable to the player
        rb = GetComponent<Rigidbody>();
        isHoldingPotato = hotPotato != null; // Check if the player is holding the potato at the start
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    // Update() is called once per frame (60-120 times per second)
    private void Update()
    {
        // Reads the player's input and stores it in the action sheet
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame())
        {
            // tells the player to jump
            HandleJump();
        }
    }

    // FixedUpdate() is called at a fixed interval (50 times per second)
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (GameManager.isGameOver) return; // if the game is over, do not allow player movement

        // calculate & store the direction the player will move based on the input
        Vector3 movementDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        //prvents diagonal movement from doubling speed since the player is using two inputs
        movementDirection.Normalize();

        // actually moves the player
        rb.MovePosition(rb.position + movementDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (GameManager.isGameOver) return; // if the game is over, do not allow player movement

        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {

        //visually draw the ray
        Debug.DrawRay(transform.position, 
        Vector3.down * groundCheckDistance);


        // checking if the player is grounded that way they cant spam jump in the air
        return Physics.Raycast(transform.position, Vector3.down, 
        groundCheckDistance, groundLayer);
    }


    /*private void OnColllsionEnter(Collision collision)
    {
        // Detect if the player collides with the other player
        Player2Controller otherPlayer = collision.gameObject.GetComponent<Player2Controller>();

        if (otherPlayer != null)
        {
            Debug.Log("Player 1 collided with Player 2!");
            // Remove the potator component from this player, then add it to the other player with the remaining time
            HotPotato hotPotato = GetComponent<HotPotato>();
            if (hotPotato != null)
            {
                float remainingTime = hotPotato.GetRemainingTime();
                Destroy(hotPotato); // Remove the HotPotato component from this player  
                // Add the HotPotato component to the other player with the remaining time
                HotPotato newHotPotato = otherPlayer.GameObject.component<HotPotato>();
                newHotPotato.SetRemainingTime(remainingTime);
            }
        }
    }*/


/*private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    if (hotPotato.IsHeldBy(transform))
    {
        hotPotato.PassToPlayer(otherPlayer);
    }
}*/

private void OnCollisionEnter(Collision collision)
    {
        Player2Controller Player2 = collision.gameObject.GetComponent<Player2Controller>();

        if (Player2 != null)
        {
            Debug.Log($"{gameObject.name} hit {collision.gameObject.name}!");
            // check if the object his has the hot potato
            if (hotPotato != null)
            {
                Debug.Log("Passing The Potato!!!!");
                // add the hotpotato to the player with the remaining time
                HotPotato newPotato = Player2.gameObject.AddComponent<HotPotato>();
                newPotato.Initialize(hotPotato.remainingTime);
                Destroy(hotPotato); // Remove the HotPotato component from this player
            }
        }
    }

}