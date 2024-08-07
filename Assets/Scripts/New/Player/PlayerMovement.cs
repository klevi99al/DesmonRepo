using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Public Variables")]
    public bool canMove = true;
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private BoxCollider2D playerCollider;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movementInput;

    private int movementHash;
    private int jumpHash;
    [SerializeField] private bool isGrounded = true;


    private bool isTouchingLeftCollider = false;
    private bool isTouchingRightCollider = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Enable player input
        InputActionMap playerActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        playerActionMap["Jump"].performed += Jump;
        playerActionMap["Movement"].performed += context => movementInput = context.ReadValue<Vector2>();
        playerActionMap["Movement"].canceled += _ => movementInput = Vector2.zero;

        movementHash = Animator.StringToHash("IsMoving");
        jumpHash = Animator.StringToHash("Jump");
    }

    private void Update()
    {
        if (canMove)
        {
            HandlePlayerMovement();
            HandlePlayerRotation();
            HandlePlayerAnimations();

            CheckSideCollisions();
        }

        isGrounded = IsGrounded();
    }

    private void CheckSideCollisions()
    {
        isTouchingLeftCollider = Physics2D.Raycast(transform.position, Vector2.left, playerCollider.bounds.extents.x + 0.1f, groundLayer);
        isTouchingRightCollider = Physics2D.Raycast(transform.position, Vector2.right, playerCollider.bounds.extents.x + 0.1f, groundLayer);
    }


    private void HandlePlayerAnimations()
    {
        animator.SetBool(movementHash, movementInput != Vector2.zero);
    }


    private void HandlePlayerMovement()
    {
        float moveInput = movementInput.x;
        Vector2 velocity = rb.velocity;

        // If the player is colliding with the left collider and trying to move left, set the horizontal velocity to 0
        if (isTouchingLeftCollider && moveInput < 0)
        {
            velocity.x = 0f;
        }
        // If the player is colliding with the right collider and trying to move right, set the horizontal velocity to 0
        else if (isTouchingRightCollider && moveInput > 0)
        {
            velocity.x = 0f;
        }
        // Otherwise, set the horizontal velocity normally
        else
        {
            velocity.x = moveInput * moveSpeed;
        }

        rb.velocity = velocity;
    }


    private void HandlePlayerRotation()
    {
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && canMove)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger(jumpHash);
            }
        }
    }

    private bool IsGrounded()
    {
        Vector2 boxCastOrigin = transform.position;
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, playerCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }
}
