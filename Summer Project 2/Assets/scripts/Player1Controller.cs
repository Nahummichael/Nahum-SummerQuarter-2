using UnityEngine;
using UnityEngine.InputSystem; 

public class Player1Controller : MonoBehaviour
{
    

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

    [SerializeField] private float passCooldown = 0.5f;
    public bool canReceivePotato = true;


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
        isHoldingPotato = GetComponent<HotPotato>() != null; // Check if the player is holding the potato at the start
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

    public void ResetPassCooldown()
    {
        canReceivePotato = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ADDED: block passing once the game has ended (this check already existed
        // in HandleMovement/HandleJump but was missing here)
        if (GameManager.isGameOver) return;

        Player2Controller player2 = collision.gameObject.GetComponent<Player2Controller>();

        // Didn't collide with Player 2
        if (player2 == null)
            return;

        Debug.Log($"{gameObject.name} hit {collision.gameObject.name}!");

        // Player 1 isn't holding the potato
        if (!isHoldingPotato)
            return;

        // Player 2 is still on cooldown
        if (!player2.canReceivePotato)
            return;

        // Safety check
        if (player2.isHoldingPotato)
            return;

        HotPotato myPotato = GetComponent<HotPotato>();

        if (myPotato == null)
            return;

        Debug.Log("Player 1 passes potato to Player 2!");

        // Give Player 2 the potato
        HotPotato newPotato = player2.gameObject.AddComponent<HotPotato>();
        newPotato.Initialize(myPotato.remainingTime);

        // Remove the potato from Player 1
        Destroy(myPotato);

        // Update holder states
        isHoldingPotato = false;
        player2.isHoldingPotato = true;

        // ADDED: put Player 1 (the giver) on cooldown too, not just Player 2.
        // Previously only the receiver got a cooldown, so the potato could
        // bounce straight back to Player 1 on the very next collision.
        canReceivePotato = false;
        Invoke(nameof(ResetPassCooldown), passCooldown);

        // Start Player 2's receive cooldown (unchanged, was already here)
        player2.canReceivePotato = false;
        player2.Invoke(nameof(player2.ResetPassCooldown), passCooldown);
    }

}