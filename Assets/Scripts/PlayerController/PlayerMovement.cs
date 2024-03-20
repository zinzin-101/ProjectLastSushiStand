using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public enum LastWall
{
    LEFT,
    RIGHT,
    BACK
}

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
    [SerializeField] float defaultWallrunGravity;
    [SerializeField] float wallHangGravity;
    [SerializeField] float wallrunSpeed;
    //[SerializeField] float wallrunSpeedDecrease;
    [SerializeField] float wallJumpBoost;
    [SerializeField] float wallrunTimer;
    [SerializeField] float wallJumpBoostWindow = 0.4f;
    [SerializeField] float wallJumpMaintainSpeedWindow = 0.25f;
    private float wallrunTimerLeft;
    private bool onRightWall, onLeftWall, onBackWall;
    private RaycastHit leftWallHit, rightWallhit, backWallhit;
    private Vector3 wallNormal;
    private Vector3 lastWallNormal;
    private LastWall lastWall;
    private bool hasWallrun = false;
    //private bool lastWallRight;
    //private bool lastWallLeft;
    private float wallrunGravity;

    private float lurchTimeLeft;

    [Header("Player Camera")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float specialFov;
    [SerializeField] float cameraTransitionTime = 8f;
    [SerializeField] float wallrunCamTiltAmount;
    [SerializeField] float fovLimit = 120f;
    private float camTilt;
    public float CamTilt => camTilt;
    private float normalFov;
    private float fovDifference;
    private float fovSpeedScaler;

    [Header("Movement switch")]
    [SerializeField] bool allowDoubleJump = true;
    [SerializeField] bool allowWallrun = true;


    private void Start()
    {
        TryGetComponent(out controller);
        defaultHeight = controller.height;
        defaultScale = transform.localScale.y;
        crouchScale = crouchHeight / defaultHeight;

        hasWallrun = false;
        //lastWallRight = false;
        //lastWallLeft = false;

        playerCamera = GetComponentInChildren<Camera>();

        normalFov = playerCamera.fieldOfView;
        fovDifference = Mathf.Max(specialFov, normalFov) - Mathf.Min(specialFov, normalFov);

        fovSpeedScaler = (sprintSpeed + wallrunSpeed) / 2f;
    }

    private void Update()
    {
        GetInput();
        ApplyGravity();

        if (allowWallrun)
        {
            CheckWallHit();
        }

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

        //if (Mathf.Abs(controller.velocity.magnitude - prevSpeed) > 5f && controller.velocity.magnitude < prevSpeed)
        //{
        //    movement = new Vector3(0f, movement.y, 0f);
        //    controller.Move(movement);
        //}


        controller.Move(movement * Time.deltaTime);

        //print(controller.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("triggered");
        //speed *= controller.velocity.magnitude / speed;

        //if (collision.gameObject.layer != groundLayer)
        //{
        //    //movement = Vector3.zero;
        //    //controller.Move(new Vector3(0f, movement.y, 0f));
        //}
    }

    private void CameraEffect()
    {
        float fov = normalFov + (fovDifference * ((speed - 5f) / fovSpeedScaler));

        if (fov > fovLimit)
        {
            fov = fovLimit;
        }


        playerCamera.fieldOfView = Mathf.MoveTowards(playerCamera.fieldOfView, fov, cameraTransitionTime * Time.deltaTime);

        if (isWallRunning)
        {
            float tiltScale = (wallrunTimerLeft / wallrunTimer);
            if (tiltScale > 0.5f)
            {
                tiltScale = 1f;
            }
            else
            {
                tiltScale += 0.5f;
            }

            if (onRightWall)
            {
                camTilt = Mathf.Lerp(camTilt, wallrunCamTiltAmount * tiltScale, cameraTransitionTime * Time.deltaTime);
            }

            if (onLeftWall)
            {
                camTilt = Mathf.Lerp(camTilt, -wallrunCamTiltAmount * tiltScale, cameraTransitionTime * Time.deltaTime);
            }
        }
        else
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
            if (isWallRunning || grounded || allowDoubleJump)
            {
                Jump();
            }
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

        movement.x += input.x * airSpeed * 0.25f;
        movement.z += input.z * airSpeed * 0.25f;

        if (speed > 15f)
        {
            speed = Mathf.MoveTowards(speed, 15f, 5f);
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
            IncreaseSpeed(wallJumpBoost / 2f);
            movement += wallNormal;
            ExitWallRun();
        }

        wallrunTimerLeft -= Time.deltaTime;

        if (!(wallrunTimer - wallrunTimerLeft <= wallJumpMaintainSpeedWindow))
        {
            speed = wallrunSpeed;
        }

        //if ((forwardDirection.z - 45f) < input.z && input.z < (forwardDirection.z + 45f) && (Mathf.Abs(input.z) > 0.5f || Mathf.Abs(input.x) > 0.5f))
        //{
        //    wallrunGravity = defaultWallrunGravity;

        //    //if (onBackWall)
        //    //{
        //    //    movement = forwardDirection;
        //    //    movement = Vector3.MoveTowards(movement, forwardDirection.normalized * speed, wallrunSpeed * Time.deltaTime);
        //    //}
        //    //else
        //    //{
        //    //    movement += forwardDirection;
        //    //    movement = Vector3.MoveTowards(movement, forwardDirection.normalized * speed, wallrunSpeed * Time.deltaTime);
        //    //}

        //    movement += forwardDirection;
        //    movement = Vector3.MoveTowards(movement, forwardDirection.normalized * speed, wallrunSpeed * Time.deltaTime);

        //    if (wallrunTimerLeft <= (wallrunTimer * 0.25f))
        //    {
        //        wallrunGravity = wallHangGravity * 0.5f;
        //    }
        //}
        //else if (input.z < (forwardDirection.z - 45f) && (forwardDirection.z + 45f) < input.z || (Mathf.Abs(input.z) < 0.5f && Mathf.Abs(input.x) < 0.5f))
        //{
        //    wallrunGravity = wallHangGravity;
        //    movement.x = Mathf.MoveTowards(movement.x, 0f, 2f * wallrunSpeed * Time.deltaTime);   
        //    movement.z = Mathf.MoveTowards(movement.z, 0f, 2f * wallrunSpeed * Time.deltaTime);
        //    //ExitWallRun();
        //}

        float angle = Vector3.Angle(input, forwardDirection);
        if (angle < 45f && ((Mathf.Abs(input.z) > 0.5f || Mathf.Abs(input.x) > 0.5f)))
        {
            wallrunGravity = defaultWallrunGravity;
            movement += forwardDirection;
            movement = Vector3.MoveTowards(movement, forwardDirection.normalized * speed, crouchSpeed * Time.deltaTime);

            if (wallrunTimerLeft <= (wallrunTimer * 0.25f))
            {
                wallrunGravity = wallHangGravity * 0.5f;
            }
        }
        else if (75f < angle && angle < 105f && ((Mathf.Abs(input.z) > 0.5f || Mathf.Abs(input.x) > 0.5f)))
        {
            wallrunGravity = defaultWallrunGravity;
            movement = Vector3.MoveTowards(movement, new Vector3(0f, movement.y, 0f), wallrunSpeed * Time.deltaTime);

            if (wallrunTimerLeft <= (wallrunTimer * 0.25f))
            {
                wallrunGravity = wallHangGravity * 0.5f;
            }
        }
        else if (angle > 125f && ((Mathf.Abs(input.z) > 0.5f || Mathf.Abs(input.x) > 0.5f)))
        {
            wallrunGravity = defaultWallrunGravity;
            movement -= forwardDirection;
            movement = Vector3.MoveTowards(movement, forwardDirection.normalized * speed, crouchSpeed * Time.deltaTime);

            if (wallrunTimerLeft <= (wallrunTimer * 0.25f))
            {
                wallrunGravity = wallHangGravity * 0.5f;
            }
        }
        else if ((Mathf.Abs(input.z) < 0.5f && Mathf.Abs(input.x) < 0.5f)){
            wallrunGravity = wallHangGravity;
            movement.x = Mathf.MoveTowards(movement.x, 0f, 2f * wallrunSpeed * Time.deltaTime);   
            movement.z = Mathf.MoveTowards(movement.z, 0f, 2f * wallrunSpeed * Time.deltaTime);
        }

        //movement.x += input.x * (wallrunSpeed / 2f);
        //movement *= wallrunSpeed;
        movement = Vector3.ClampMagnitude(movement, speed);
    }

    private void Jump()
    {
        jumpCharges--;
        if (!grounded && !isWallRunning)
        {
            jumpCharges--;
        }

        if (isWallRunning)
        {
            ExitWallRun();

            movement.x += input.x;
            movement.z += input.z;
            movement += onBackWall ? wallNormal * 5f : wallNormal * 2.5f;

            if (wallrunTimer - wallrunTimerLeft <= wallJumpBoostWindow)
            {
                IncreaseSpeed(wallJumpBoost * 1.5f);
            }
            else
            {
                IncreaseSpeed(wallJumpBoost);
            }
        }
        else if (!grounded)
        {
            Vector3 currentMovingDirection = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
            float angleChange = Vector3.Angle(input, currentMovingDirection);
            float jumpSpeed = walkSpeed * 0.25f;
            if (angleChange > 90f)
            {
                movement.x = input.x * jumpSpeed;
                movement.z = input.z * jumpSpeed;
            }
            else
            {
                movement.x += input.x * jumpSpeed;
                movement.z += input.z * jumpSpeed;
            }
            
        }

        YVelocity.y = Mathf.Sqrt(jumpHeight * -2f * defaultGravity);
        /* v2 = u2 + 2gs
           v2 = 0 + 2gs
            v = sqrt(2gs) // g is negative
         */

        lurchTimeLeft = lurchTimer;
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
        //if (-0.5f <= controller.velocity.y && controller.velocity.y <= 0.5f)
        //{
        //    return;
        //}

        if (isCrouching)
        {
            IncreaseSpeed(wallJumpBoost / 2f);
            movement += wallNormal;
            return;
        }

        Vector3 currentMovingDirection = new Vector3(controller.velocity.x, 0f, controller.velocity.z);

        wallrunGravity = defaultWallrunGravity;
        jumpCharges = defaultJumpCharges;
        wallrunTimerLeft = wallrunTimer;

        YVelocity = new Vector3(0f, 0f, 0f);

        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(forwardDirection, transform.forward) < 0f)
        {
            forwardDirection = -forwardDirection;
        }

        //movement += -wallNormal.normalized * 2f;

        float angleChange = Vector3.Angle(forwardDirection, currentMovingDirection);
        if (angleChange > 80f)
        {
            movement = Vector3.zero;
            speed = 0f;
        }
        else if (angleChange > 45f)
        {
            speed *= 1f - (angleChange / 180f);
        }

        isWallRunning = true;
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
        //grounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.5f, groundLayer);

        if (grounded)
        {
            jumpCharges = defaultJumpCharges;
            hasWallrun = false;
        }
    }

    private void CheckWallHit()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.8f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallhit, 0.8f, wallLayer);
        onBackWall = Physics.Raycast(transform.position, -transform.forward, out backWallhit, 0.8f, wallLayer);

        if ((onLeftWall || onRightWall || onBackWall) && !isWallRunning)
        {
            CheckLastWallrun();
        }

        if (!(onRightWall || onLeftWall || onBackWall) && isWallRunning)
        {
            if (onRightWall)
            {
                lastWall = LastWall.RIGHT;
            }
            else if (onLeftWall)
            {
                lastWall = LastWall.LEFT;
            }
            else if (onBackWall)
            {
                lastWall = LastWall.BACK;
            }

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
        else if (onBackWall)
        {
            wallNormal = lastWallNormal;
        }

        //if ((lastWallRight && onLeftWall) || (lastWallLeft && onRightWall))
        //{
        //    hasWallrun = false;
        //}

        if (hasWallrun)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWallNormal);

            if (wallAngle > 1f || ((onBackWall && lastWall == LastWall.BACK) || (wallNormal != lastWallNormal)))
            {
                EnterWallRun();
                return;
            }

            return;
        }

        EnterWallRun();

        //lastWallRight = onRightWall;
        //lastWallLeft = onLeftWall;

        hasWallrun = true;
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

    public void SetAllowWallRun(bool input)
    {
        allowWallrun = input;
    }

    public void SetAllowDoubleJump(bool input)
    {
        allowDoubleJump = input;
    }
}