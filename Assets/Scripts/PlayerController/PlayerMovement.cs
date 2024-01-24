using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;


    [SerializeField] float jumpForce;
    [SerializeField] float jumpCoolDown;
    [SerializeField] float airMultiplier;
    private bool canJump;

    [SerializeField] float groundDrag;
    [SerializeField] float stoppingDrag = 20f;


    [Header("GroundCheck")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayer;
    private bool grounded;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [SerializeField] Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        TryGetComponent(out rb);
        rb.freezeRotation = true;

        canJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        GetInput();

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
        SpeedControl();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetCanJump), jumpCoolDown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (grounded)
        {
            Vector3 newVector = rb.velocity;

            if (moveDirection.x * rb.velocity.x <= 0)
            {
                newVector.x = 0;
            }

            if (moveDirection.z * rb.velocity.z <= 0)
            {
                newVector.z = 0;
            }

            rb.velocity = newVector;
            rb.AddForce(moveDirection * moveSpeed * 5f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }


        print(rb.velocity.y);
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
