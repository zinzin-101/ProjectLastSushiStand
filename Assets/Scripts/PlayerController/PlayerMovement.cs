using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    private Vector3 movement;
    private Vector3 input;

    [Header("KeyBind")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float defaultGravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float crouchHeight;
    private float defaultHeight;
    private float defaultScale, crouchScale;
    private float speed;

    [Header("Ground")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    private Vector3 YVelocity;
    private int jumpCharges;
    private bool grounded;

    private float gravity;

    private bool isSprinting;
    private bool isCrouching;
    private bool isSliding;

    [Header("Sliding")]
    //[SerializeField] float maxSlideTimer;
    [SerializeField] float slideSpeedUp;
    [SerializeField] float slideSpeedDown;
    //private Vector3 forwardDirection;
    //private float slideTimer;

    //[SerializeField] Vector3 crouchingCenter = new Vector3(0f, 0.5f, 0f);
    //[SerializeField] Vector3 standingCenter = new Vector3(0f, 0f, 0f);

    [SerializeField] float lurchTimer;
    private float lurchTimeLeft;

    private void Start()
    {
        TryGetComponent(out controller);
        defaultHeight = controller.height;
        defaultScale = transform.localScale.y;
        crouchScale = crouchHeight / defaultHeight;
    }

    private void Update()
    {
        GetInput();
        ApplyGravity();
        CheckGround();

        if (grounded && !isSliding)
        {
            GroundedMovement();
        }
        else if (!grounded)
        {
            AirMovement();
        }
        else if (isSliding)
        {
            SlideMovement();
            DecreaseSpeed(slideSpeedDown);

            //slideTimer -= Time.deltaTime;
            //if (slideTimer < 0)
            //{
            //    isSliding = false;
            //}

            if (speed <= walkSpeed)
            {
                isSliding = false;
            }
        }

        controller.Move(movement * Time.deltaTime);

        print(controller.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0f;
    }

    private void GetInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetKeyDown(jumpKey) && jumpCharges > 0)
        {
            Jump();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            EnterCrouch();
        }
        if (Input.GetKeyUp(crouchKey))
        {
            ExitCrouch();
        }

        if (Input.GetKey(sprintKey) && grounded && Input.GetAxisRaw("Vertical") > 0f && !isCrouching)
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(sprintKey) || Input.GetAxisRaw("Vertical") <= 0f)
        {
            isSprinting = false;
        }
    }

    private void GroundedMovement()
    {
        if (isSprinting)
        {
            speed = sprintSpeed;
        }
        else if (isCrouching)
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (input.x != 0f)
        {
            movement.x += input.x * speed;
        }
        else
        {
            movement.x = 0f;

        }

        if (input.z != 0f)
        {
            movement.z += input.z * speed;
        }
        else
        {
            movement.z = 0f;

        }

        movement = Vector3.ClampMagnitude(movement, speed);

        lurchTimeLeft = lurchTimer;
    }

    private void AirMovement()
    {
        if (lurchTimeLeft > 0f)
        {
            movement.x += input.x * airSpeed;
            movement.z += input.z * airSpeed;

            lurchTimeLeft -= Time.deltaTime;
        }

        movement = Vector3.ClampMagnitude(movement, speed * 1.1f);
    }

    private void SlideMovement()
    {
        //movement += forwardDirection;
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    private void Jump()
    {
        jumpCharges--;
        YVelocity.y = Mathf.Sqrt(jumpHeight * -2f * defaultGravity);
        /* v2 = u2 + 2gs
           v2 = 0 + 2gs
            v = sqrt(2gs) // g is negative
         */
    }

    private void EnterCrouch()
    {
        controller.height = crouchHeight;
        //controller.center = crouchingCenter;

        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
        isCrouching = true;

        if (speed > walkSpeed)
        {
            isSliding = true;
            //forwardDirection = transform.forward;

            if (grounded)
            {
                IncreaseSpeed(slideSpeedUp);
            }

            //slideTimer = maxSlideTimer;
        }
    }

    private void ExitCrouch()
    {
        controller.height = defaultHeight;
        //controller.center = standingCenter;

        transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);
        isCrouching = false;
        isSliding = false;
    }

    private void IncreaseSpeed(float addSpeed)
    {
        speed += addSpeed;
    }

    private void DecreaseSpeed(float reduceSpeed)
    {
        if (controller.velocity.y < 0f)
        {
            return;
        }

        speed -= reduceSpeed * Time.deltaTime;
    }

    private void CheckGround()
    {
        grounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (grounded)
        {
            jumpCharges = 1;
        }
    }

    private void ApplyGravity()
    {
        gravity = grounded ? 0f : defaultGravity;
        YVelocity.y += gravity * Time.deltaTime;
        controller.Move(YVelocity * Time.deltaTime);
    }
}
