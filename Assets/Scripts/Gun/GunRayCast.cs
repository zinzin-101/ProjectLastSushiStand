using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRayCast : MonoBehaviour
{
    //[SerializeField] private int damage = 10;
    [SerializeField] private int range = 100;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private int ammo = 6;
    [SerializeField] private bool capacity = true;
    private void Update()
    {
        if (capacity)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
                ammo--;
                if (ammo == 0)
                {
                    capacity = false;
                    Debug.Log("Press R to reload");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloaded");
            ammo = 6;
            capacity = true;
        }
    }
    
    void Fire()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
