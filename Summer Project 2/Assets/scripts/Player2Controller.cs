using UnityEngine;
using UnityEngine.InputSystem; //imports the input system into this script

public class Player2Controller : MonoBehaviour
{
    

    [SerializeField, Tooltip("A variable to store the input action sheet the player needs for inputs")] 
    private InputActionAsset InputActions;

    //THE ACTUAL ACTIONS
    private InputAction moveAction2;
    private InputAction jumpAction2;

    //LOGIC
    private Vector2 moveInput2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float groundCheckDistance = 1f;


    //Components
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float passCooldown = 0.5f;
    public bool canReceivePotato = true;


    //player settings 
    [SerializeField, Tooltip("the speed the player moves at")] private float moveSpeed = 5f;
    [SerializeField, Tooltip("the player's jump height")] private float jumpForce = 5f;
    public bool isHoldingPotato = false; // New variable to track if the player is holding the potato

    // awake is called when the instance first loads
    private void Awake()
    {
        //assigns the input action variables to their actual input action
        moveAction2 = InputSystem.actions.FindAction("Move2");
        jumpAction2 = InputSystem.actions.FindAction("Jump2");

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
        moveInput2 = moveAction2.ReadValue<Vector2>();

        if (jumpAction2.WasPressedThisFrame())
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
        Vector3 movementDirection = transform.forward * moveInput2.y + transform.right * moveInput2.x;

        //prvents diagonal movement from doubling speed since the player is using two inputs
        movementDirection.Normalize();

         float moveDistance = moveSpeed * Time.fixedDeltaTime;
        float rayDistance = moveDistance + 0.5f;

        if (Physics.Raycast(transform.position, movementDirection, rayDistance, wallLayer))
        {
            return; // wall ahead — stop movement completely
        }

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

        Player1Controller player1 = collision.gameObject.GetComponent<Player1Controller>();

        // Didn't collide with Player 2
        if (player1 == null)
            return;

        Debug.Log($"{gameObject.name} hit {collision.gameObject.name}!");

        // Player 1 isn't holding the potato
        if (!isHoldingPotato)
            return;

        // Player 2 is still on cooldown
        if (!player1.canReceivePotato)
            return;

        // Safety check
        if (player1.isHoldingPotato)
            return;

        HotPotato myPotato = GetComponent<HotPotato>();

        if (myPotato == null)
            return;

        Debug.Log("Player 1 passes potato to Player 2!");

        // Give Player 2 the potato
        HotPotato newPotato = player1.gameObject.AddComponent<HotPotato>();
        newPotato.Initialize(myPotato.remainingTime);

        // Remove the potato from Player 1
        Destroy(myPotato);

        // Update holder states
        isHoldingPotato = false;
        player1.isHoldingPotato = true;

        // ADDED: put Player 1 (the giver) on cooldown too, not just Player 2.
        // Previously only the receiver got a cooldown, so the potato could
        // bounce straight back to Player 1 on the very next collision.
        canReceivePotato = false;
        Invoke(nameof(ResetPassCooldown), passCooldown);

        // Start Player 2's receive cooldown (unchanged, was already here)
        player1.canReceivePotato = false;
        player1.Invoke(nameof(player1.ResetPassCooldown), passCooldown);
    }
}