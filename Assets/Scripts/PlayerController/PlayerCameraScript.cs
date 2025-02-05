using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] float sensitivity;

    private Transform camTransform;
    [SerializeField] Transform orientation;

    private float rotationX;
    private float rotationY;

    private void Start()
    {
        //playerMovement = GetComponentInParent<PlayerMovement>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        //rotate the camera and rotate the player object towards the camera
        transform.rotation = Quaternion.Euler(rotationX, rotationY, playerMovement.CamTilt);

        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
