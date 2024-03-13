using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
//using static UnityEditor.Experimental.GraphView.Port;

public class GunParticle : MonoBehaviour
{
    //public UnityEvent isShooting;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Animator animator;
    //public float Cooldown = 0.1f;
    //public float CurrentCooldown;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private bool IneedMoreBullet = true;
    [SerializeField] private int setAmmo = 6;

    private int ammo;

    private void Start()
    {
        //CurrentCooldown = Cooldown;
        animator = gameObject.GetComponent<Animator>();
        ammo = setAmmo;
    }

    

    private void Update()
    {
        if (IneedMoreBullet)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Shoot");
                ammo--;
                //if (CurrentCooldown <= 0f)
                //{
                //    isShooting?.Invoke();
                //    CurrentCooldown = Cooldown;
            
                if (IneedMoreBullet)
                {
                    Quaternion rota = Quaternion.Euler(bulletSpawnPoint.rotation.x, bulletSpawnPoint.rotation.y, bulletSpawnPoint.rotation.z + 90);
                    var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
                    if (ammo == 0)
                    {
                        IneedMoreBullet = false;
                        Debug.Log("Press R to reload");
                    }
                }
                //CurrentCooldown -= Time.deltaTime;
            }
        }
    
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloaded");
            ammo = setAmmo;
            IneedMoreBullet = true;
        }


    }
    public int getAmmo()
    {
        return ammo;
    }
}
