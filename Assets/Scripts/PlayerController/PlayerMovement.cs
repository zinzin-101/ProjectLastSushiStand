using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] int defaultJumpCharges = 1;
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
    private bool isWallRunning;

    [SerializeField] float lurchTimer;

    [Header("Sliding")]
    //[SerializeField] float maxSlideTimer;
    [SerializeField] float slideSpeedUp;
    [SerializeField] float slideSpeedDown;
    private Vector3 forwardDirection;
    //private float slideTimer;

    //[SerializeField] Vector3 crouchingCenter = new Vector3(0f, 0.5f, 0f);
    //[SerializeField] Vector3 standingCenter = new Vector3(0f, 0f, 0f);

    [Header("Wallrun")]
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallrunGravity;
    [SerializeField] float wallrunSpeed;
    //[SerializeField] float wallrunSpeedDecrease;
    [SerializeField] float wallJumpBoost;
    [SerializeField] float wallrunTimer;
    private float wallrunTimerLeft;
    private bool onRightWall, onLeftWall;
    private RaycastHit leftWallHit, rightWallhit;
    private Vector3 wallNormal;
    private Vector3 lastWallNormal;
    private bool hasWallrun = false;

    private float lurchTimeLeft;

    [Header("Player Camera")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float specialFov;
    [SerializeField] float cameraTransitionTime;
    [SerializeField] float wallrunCamTiltAmount;
    private float camTilt;
    public float CamTilt => camTilt;
    private float normalFov;


    private void Start()
    {
        TryGetComponent(out controller);
        defaultHeight = controller.height;
        defaultScale = transform.localScale.y;
        crouchScale = crouchHeight / defaultHeight;

        hasWallrun = false;

        playerCamera = GetComponentInChildren<Camera>();

        normalFov = playerCamera.fieldOfView;
    }

    private void Update()
    {
        GetInput();
        ApplyGravity();
        CheckWallHit();
        CheckGround();

        CameraEffect();

        if (grounded && !isSliding)
        {
            GroundedMovement();
        }
        else if (!grounded && !isWallRunning)
        {
            AirMovement();
        }
        else if (isSliding)
        {
            SlideMovement();
            DecreaseSpeedOvertime(slideSpeedDown);

            //slideTimer -= Time.deltaTime;
            //if (slideTimer < 0)
            //{
            //    isSliding = false;
            //}

            if (speed <= 0f || (Input.GetAxisRaw("Vertical") > 0f && speed <= crouchSpeed))
            {
                isSliding = false;
            }
        }
        else if (isWallRunning)
        {
            WallRunningMovement();     
        }

        controller.Move(movement * Time.deltaTime);

        print(controller.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //speed = 0f;
    }

    private void CameraEffect()
    {
        float fov;

        if (isWallRunning || isSliding)
        {
            fov = specialFov;
        }
        else
        {
            fov = normalFov;
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, cameraTransitionTime * Time.deltaTime);

        if (isWallRunning )
        {
            if (onRightWall)
            {
                camTilt = Mathf.Lerp(camTilt, wallrunCamTiltAmount, cameraTransitionTime * Time.deltaTime);
            }

            if (onLeftWall)
            {
                camTilt = Mathf.Lerp(camTilt, -wallrunCamTiltAmount, cameraTransitionTime * Time.deltaTime);
            }
        }

        if (!isWallRunning )
        {
            camTilt = Mathf.Lerp(camTilt, 0f, cameraTransitionTime * Time.deltaTime);
        }
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

        if (Input.GetKey(sprintKey) && grounded && Input.GetAxisRaw("Vertical") > 0f && !isCrouching && !isSliding)
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
        if (isCrouching)
        {
            speed = crouchSpeed;
        }
        else if (isSprinting)
        {
            speed = sprintSpeed;
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

        movement = Vector3.ClampMagnitude(movement, speed);
    }

    private void SlideMovement()
    {
        if (!grounded)
        {
            return;
        }

        if (controller.velocity.y < -1f)
        {
            speed += slideSpeedDown * (-controller.velocity.y / 4f) * Time.deltaTime;
        }
        else if (controller.velocity.y > 1f)
        {
            speed -= slideSpeedDown * (controller.velocity.y / 2f) * Time.deltaTime;
        }

        movement += forwardDirection;
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    private void WallRunningMovement()
    {
        if (wallrunTimerLeft <= 0f)
        {
            ExitWallRun();
            IncreaseSpeed(wallJumpBoost);
            movement += wallNormal;
        }

        wallrunTimerLeft -= Time.deltaTime;

        if ((forwardDirection.z - 10f) < input.z && input.z < (forwardDirection.z + 10f))
        {
            movement.x += forwardDirection.x;
            movement.z += forwardDirection.z;
        }
        else if (input.z < (forwardDirection.z - 10f) && (forwardDirection.z + 10f) < input.z)
        {
            movement.x = 0f;
            movement.z = 0f;
            ExitWallRun();
        }

        movement.x += input.x * (wallrunSpeed / 2f);

        movement = Vector3.ClampMagnitude(movement, speed);
    }

    private void Jump()
    {
        if (!grounded && !isWallRunning)
        {
            jumpCharges--;
        }
        else if (isWallRunning)
        {
            ExitWallRun();
            IncreaseSpeed(wallJumpBoost);
            movement += wallNormal * 2f;
        }

        YVelocity.y = Mathf.Sqrt(jumpHeight * -2f * defaultGravity);
        /* v2 = u2 + 2gs
           v2 = 0 + 2gs
            v = sqrt(2gs) // g is negative
         */
    }

    private void EnterCrouch()
    {
        if (isWallRunning)
        {
            ExitWallRun();
        }

        controller.height = crouchHeight;
        //controller.center = crouchingCenter;

        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
        isCrouching = true;


        if (speed > walkSpeed)
        {
            isSliding = true;
            forwardDirection = movement.normalized;

            if (grounded)
            {
                IncreaseSpeed(slideSpeedUp);
            }

            //slideTimer = maxSlideTimer;
        }

        if (isSliding)
        {
            ExitWallRun();
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

    private void EnterWallRun()
    {
        isWallRunning = true;
        jumpCharges = defaultJumpCharges;
        wallrunTimerLeft = wallrunTimer;

        speed = wallrunSpeed;

        YVelocity = new Vector3(0f, 0f, 0f);

        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(forwardDirection, transform.forward) < 0f)
        {
            forwardDirection = -forwardDirection;
        }
    }

    private void ExitWallRun()
    {
        isWallRunning = false;
        lastWallNormal = wallNormal;
    }

    private void IncreaseSpeed(float addSpeed)
    {
        speed += addSpeed;
    }

    private void DecreaseSpeedOvertime(float reduceSpeed)
    {
        speed -= reduceSpeed * Time.deltaTime;
    }

    private void CheckGround()
    {
        grounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (grounded)
        {
            jumpCharges = defaultJumpCharges;
            hasWallrun = false;
        }
    }

    private void CheckWallHit()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallhit, 0.7f, wallLayer);

        if ((onLeftWall || onRightWall) && !isWallRunning)
        {
            CheckLastWallrun();
        }

        if (!(onRightWall || onLeftWall) && isWallRunning)
        {
            ExitWallRun();
        }
    }
    
    private void CheckLastWallrun()
    {
        if (onLeftWall)
        {
            wallNormal = leftWallHit.normal;
        }
        else if (onRightWall)
        {
            wallNormal = rightWallhit.normal;
        }

        if (hasWallrun)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWallNormal);

            if (wallAngle > 15f)
            {
                EnterWallRun();
                return;
            }

            return;
        }

        if (isCrouching)
        {
            IncreaseSpeed(wallJumpBoost);
            movement += wallNormal;
        }
        else
        {
            EnterWallRun();
            hasWallrun = true;
        }
    }

    private void ApplyGravity()
    {
        if (isWallRunning)
        {
            gravity = wallrunGravity;
        }
        else
        {
            gravity = grounded ? 0f : defaultGravity;
        }

        YVelocity.y += gravity * Time.deltaTime;
        controller.Move(YVelocity * Time.deltaTime);
    }
}