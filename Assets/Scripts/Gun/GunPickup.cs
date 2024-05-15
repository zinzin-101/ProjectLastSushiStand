using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            GunScript gunScript = plrScript.GetComponentInChildren<GunScript>();
            gunScript.ActivateGun(1, true);
            print("activated");
            Destroy(gameObject);
        }
    }
}
