using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRayCast : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int range = 100;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadDelay = 1.5f;
    public int MaxAmmo => maxAmmo;
    private int ammoCount;
    public int AmmoCount => ammoCount;
    private bool canShoot;
    private bool isReloading;
    public bool IsReloading => isReloading;

    [SerializeField] private Transform gunPos;
    [SerializeField] private TrailRenderer bulletTrail;

    private void Start()
    {
        ammoCount = maxAmmo;
        canShoot = true;
        isReloading = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canShoot)
            {
                Fire();
                ammoCount--;
                if (ammoCount <= 0)
                {
                    canShoot = false;
                    //Debug.Log("Press R to reload");
                }
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("Reloaded");
            StartCoroutine(Reload());
        }
    }
    
    void Fire()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            TrailRenderer trail = Instantiate(bulletTrail, gunPos.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

            //Debug.Log(hit.transform.name);
            if (hit.collider.gameObject.TryGetComponent(out EnemyHp enemyHp))
            {
                enemyHp.TakeDamage(damage);
            }
        }
    }

    IEnumerator Reload()
    {
        if (!isReloading)
        {
            isReloading = true;

            yield return new WaitForSeconds(reloadDelay);

            ammoCount = maxAmmo;
            canShoot = true;

            isReloading = false;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }
}
