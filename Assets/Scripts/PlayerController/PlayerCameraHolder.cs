using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHolder : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    private void Update()
    {
        transform.position = cameraTransform.position;
    }
}
