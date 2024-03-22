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

            Vector3 newPos = new Vector3(modelTransform.position.x, modelTransform.position.y, modelTransform.position.z);

            newPos.x = Mathf.Lerp(newPos.x, transform.position.x, smoothingSpeed);
            newPos.y = Mathf.Lerp(newPos.y, transform.position.y, smoothingSpeed);
            newPos.z = Mathf.Lerp(newPos.z, transform.position.z, smoothingSpeed);
            modelTransform.position = newPos;
            //modelTransform.position = Vector3.MoveTowards(modelTransform.position, newPos, smoothingSpeed);
            modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, transform.rotation, smoothingSpeed);
        }
    }
}
