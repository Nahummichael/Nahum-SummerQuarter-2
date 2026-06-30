using UnityEngine;
using UnityEngine.InputSystem; //imports the input system into this script

public class Player2 : MonoBehaviour
{
    [SerializeField, Tooltip("A variable to store the input action sheet the player needs for inputs")] 
    private InputActionAsset InputActions;

    //THE ACTUAL ACTIONS
    private InputAction moveAction2;
    private InputAction jumpAction2;

    //LOGIC
    private Vector2 moveInput2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1f;


    //Components
    [SerializeField] private Rigidbody rb;


    //player settings 
    [SerializeField, Tooltip("the speed the player moves at")] private float moveSpeed = 5f;
    [SerializeField, Tooltip("the player's jump height")] private float jumpForce = 5f;

    // awake is called when the instance first loads
    private void Awake()
    {
        //assigns the input action variables to their actual input action
        moveAction2 = InputSystem.actions.FindAction("Move2");
        jumpAction2 = InputSystem.actions.FindAction("Jump2");

        //assign the rb variable to the player
        rb = GetComponent<Rigidbody>();
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
        // calculate & store the direction the player will move based on the input
        Vector3 movementDirection = transform.forward * moveInput2.y + transform.right * moveInput2.x;

        //prvents diagonal movement from doubling speed since the player is using two inputs
        movementDirection.Normalize();

        // actually moves the player
        rb.MovePosition(rb.position + movementDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
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

}