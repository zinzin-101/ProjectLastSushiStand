using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCoolDown;
    [SerializeField] float airMultiplier;
    private bool canJump;

    [Header("Crouch")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    private float normalYScale;

    [Header("Drag")]
    private float groundDrag;
    [SerializeField] float walkDrag = 1f;
    [SerializeField] float sprintDrag = 3f;
    [SerializeField] float stoppingDrag = 20f;

    [Header("GroundCheck")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayer;
    private bool grounded;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [SerializeField] Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air
    }

    private MovementState currentState;


    private void Start()
    {
        TryGetComponent(out rb);
        rb.freezeRotation = true;

        canJump = true;
        currentState = MovementState.Walking;

        normalYScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        GetInput();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            if (moveDirection.normalized.magnitude == 0)
            {
                rb.drag = stoppingDrag;
            }
            else
            {
                rb.drag = groundDrag;
            }
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StateHandler()
    {
        if (Input.GetKey(crouchKey))
        {
            currentState = MovementState.Crouching;
            moveSpeed = crouchSpeed;
            groundDrag = walkDrag;
            return;
        }

        if (grounded && Input.GetKey(sprintKey) && verticalInput > 0)
        {
            currentState = MovementState.Sprinting;
            moveSpeed = sprintSpeed;
            groundDrag = sprintDrag;
            return;
        }
        
        if (grounded)
        {
            currentState = MovementState.Walking;
            moveSpeed = walkSpeed;
            groundDrag = walkDrag;
            return;
        }

        currentState = MovementState.Air;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetCanJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            
            if (grounded)
            {
                rb.AddForce(5f * Vector3.down, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, normalYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (grounded)
        {
            rb.AddForce(5f * moveSpeed * moveDirection, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }


        print(rb.velocity.magnitude);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void ResetCanJump()
    {
        canJump = true;
    }

    private void SpeedControl()
    {
        Vector3 currentSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float currentYSpeed = rb.velocity.y;

        if (currentSpeed.magnitude > moveSpeed * 5f && grounded)
        {
            Vector3 newSpeed = currentSpeed.normalized * moveSpeed;
            rb.velocity = new Vector3(newSpeed.x, currentYSpeed, newSpeed.z);
        }
    }
}
