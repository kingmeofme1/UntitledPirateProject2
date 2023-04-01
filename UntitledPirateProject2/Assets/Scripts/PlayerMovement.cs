using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // InputActions
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private KeyCode walkKey;

    // Input values
    private Vector2 movementInput;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // Movement values
    [SerializeField]
    private float walkSpeed = 15f;
    [SerializeField]
    private float runMultiplier = 1f;
    [SerializeField]
    private float collisionOffset = 0.2f;
    [SerializeField]
    private ContactFilter2D movementFilter;

    private float moveSpeed;

    public bool isMoving { get; private set; }

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        movementInput = movement.action.ReadValue<Vector2>();
        // Run check
        moveSpeed = (Input.GetKey(walkKey)) ? walkSpeed : runMultiplier * walkSpeed;
    }

    void FixedUpdate()
    {
        // Do nothing if idle.
        if (movementInput == Vector2.zero) 
        {
            isMoving = false;
            return;
        }
        // Allow for one directional movement if trying to move diagonally and there is something blocking one direction
        
        isMoving = true;
        bool success = TryMove(movementInput);  

        if (!success)
        {
            // try to move in the x direction
            success = TryMove(new Vector2(movementInput.x, 0));
        }

        if (!success)
        {
            // try to move in the y direction
            success = TryMove(new Vector2(0, movementInput.y));
        }

        // Check for flipping
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false;
        }

        // Check for potential collisions along the direction of the player's movement at a distance of the collisionOffset
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.deltaTime + collisionOffset);

        // Move if there are no potential collisions
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }
}
