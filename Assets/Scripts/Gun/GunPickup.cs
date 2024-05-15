using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out GunScript gunScript))
        {
            gunScript.ActivateGun(1, true);
            print("activated");
            Destroy(gameObject);
        }
    }
}
