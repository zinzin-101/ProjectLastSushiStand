using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (cameraTransform != null)
        {
            Vector3 directionToCamera = cameraTransform.transform.position - transform.position;
            directionToCamera.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera, cameraTransform.transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
