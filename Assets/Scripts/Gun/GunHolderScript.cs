using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolderScript : MonoBehaviour
{
    [SerializeField] GameObject gunModel;
    [SerializeField] float smoothingSpeed = 10f;
    private Transform modelTransform;

    private void Start()
    {
        if (gunModel != null)
        {
            modelTransform = gunModel.GetComponent<Transform>();
            modelTransform.localScale = Vector3.MoveTowards(modelTransform.position, transform.localScale, smoothingSpeed);
            modelTransform.position = transform.position;
            modelTransform.rotation = transform.rotation;
        }
    }

    private void Update()
    {
        if (gunModel != null)
        {
            modelTransform.localScale = transform.localScale;
            modelTransform.position = Vector3.MoveTowards(modelTransform.position, transform.position, smoothingSpeed);
            modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, transform.rotation, smoothingSpeed);
        }
    }
}
