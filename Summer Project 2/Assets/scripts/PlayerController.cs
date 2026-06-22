using UnityEngine;
using UnityEngine.InputSystem; //imports the input system into this script

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("A variable to store the input action sheet the player needs for inputs")] 
    private InputActionAsset InputActions;

    // these are the actual actions
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveInput;

    //Components
    [SerializeField] private Rigidbody rb;


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
        // calculate & store the direction the player will move based on the input
        Vector3 movementDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        //prvents diagonal movement from doubling speed since the player is using two inputs
        movementDirection.Normalize();

        // actually moves the player
        rb.MovePosition(rb.position + movementDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        Debug.Log("Jump My Bones!");
    }
}
