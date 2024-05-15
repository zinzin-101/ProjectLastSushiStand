using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;

    [SerializeField] private ParticleSystem ShootingSystem;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] List<GunObj> gunList;
    private int currentIndex, prevIndex;
    [SerializeField] bool[] activateIndex;

    [SerializeField] KeyCode swapGunKey = KeyCode.Q;
    [SerializeField] float recoilReductionScaling = 0.5f;

    [SerializeField] UIScript ui;

    private Transform gunPos;
    private int damage;
    private float critMultiplier;
    private int range;
    private int maxAmmo;
    private float reloadDelay;
    private float firerate;
    private float firingDelay;
    private bool canFire;
    public int MaxAmmo => maxAmmo;
    private int ammoCount;
    public int AmmoCount => ammoCount;
    private bool canShoot;
    private bool isReloading;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        transform.parent.gameObject.TryGetComponent(out playerMovement);

        ui = FindFirstObjectByType<UIScript>();
    }

    private void Start()
    {
        currentIndex = 0;
        prevIndex = 0;

        foreach (GunObj gun in gunList)
        {
            gun.InitAnimator();
            gun.SetCurrentAmmo(gun.GunInfo.maxAmmo);
            gun.GunModel.SetActive(false);
        }

        InitGun(true);
    }

    private void Update()
    {
        if (!canFire)
        {
            firingDelay -= Time.deltaTime;

            if (firingDelay <= 0f)
            {
                canFire = true;
                //canShoot = true;
            }
        }
        else
        {
            firingDelay = firerate;
        }



        if (Input.GetKey(KeyCode.Mouse0) && canFire)
        {
            canFire = false;
            firingDelay = firerate;
            if (canShoot)
            {
                Fire();
                ammoCount--;
                //canShoot = false;
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

        if (Input.GetKeyDown(swapGunKey))
        {
            SwitchGun();
        }
    }

    void Fire()
    {
        if(Time.timeScale > 0f)
        {
            gunList[currentIndex].TriggerFireAnim();

            RaycastHit hit;
            ParticleSystem effect = Instantiate(ShootingSystem, gunPos.position, Quaternion.identity);
            effect.transform.parent = this.transform;
            effect.Play();
            SoundManager.PlaySound(SoundManager.Sound.AutoRifle);

            Vector3 dir = fpsCam.transform.forward;
            float spreadAmount = gunList[currentIndex].GunInfo.spreadAmount;

            if (playerMovement != null)
            {
                if (playerMovement.IsCrouching)
                {
                    spreadAmount *= recoilReductionScaling;
                }
            }

            if (spreadAmount != 0f)
            {
                dir.x += Random.Range(-spreadAmount, spreadAmount);
                dir.y += Random.Range(-spreadAmount, spreadAmount);
                dir.z += Random.Range(-spreadAmount, spreadAmount);
            }

            if (Physics.Raycast(fpsCam.transform.position, dir, out hit, range))
            {
                // Instantiate bullet trail
                TrailRenderer trail = Instantiate(bulletTrail, gunPos.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                // Instantiate shooting particle system


                Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                // Instantiate impact particle system if hit enemy
                if (hit.collider.gameObject.TryGetComponent(out EnemyHp enemyHp))
                {
                    enemyHp.TakeDamage(damage);
                    SoundManager.PlaySound(SoundManager.Sound.EnemyHitted);
                    if (ui != null)
                    {
                        ui.TriggerMarker();
                    }
                }
                if (hit.collider.gameObject.TryGetComponent(out HeadEnemy headEnemy))
                {
                    //headEnemy.Headshot(damage);
                    headEnemy.Headshot((int)((float)damage * critMultiplier));
                    SoundManager.PlaySound(SoundManager.Sound.EnemyHitted);
                    if (ui != null)
                    {
                        ui.TriggerCritMarker();
                    }
                    //enemyHp.TakeDamage((int)((float)damage * critMultiplier));
                }


            }
        }
        
    }

    IEnumerator Reload()
    {
        if (!isReloading && ammoCount < maxAmmo)
        {
            isReloading = true;
            SoundManager.PlaySound(SoundManager.Sound.ReloadAssult);
            yield return new WaitForSeconds(reloadDelay);

            ammoCount = maxAmmo;
            canShoot = true;

            isReloading = false;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0.0f;
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

    void InitGun()
    {
        gunList[prevIndex].GunModel.SetActive(false);
        gunList[currentIndex].GunModel.SetActive(true);

        canShoot = true;
        isReloading = false;
        canFire = true;

        gunPos = gunList[currentIndex].GunPos;

        damage = gunList[currentIndex].GunInfo.damage;
        critMultiplier = gunList[currentIndex].GunInfo.critMultiplier;
        range = gunList[currentIndex].GunInfo.range;
        maxAmmo = gunList[currentIndex].GunInfo.maxAmmo;
        reloadDelay = gunList[currentIndex].GunInfo.reloadDelay;
        firerate = gunList[currentIndex].GunInfo.firerate;

        gunList[prevIndex].SetCurrentAmmo(ammoCount);
        ammoCount = gunList[currentIndex].CurrentAmmoCount;
    }

    void InitGun(bool init)
    {
        gunList[prevIndex].GunModel.SetActive(false);
        gunList[currentIndex].GunModel.SetActive(true);

        canShoot = true;
        isReloading = false;
        canFire = true;

        gunPos = gunList[currentIndex].GunPos;

        damage = gunList[currentIndex].GunInfo.damage;
        critMultiplier = gunList[currentIndex].GunInfo.critMultiplier;
        range = gunList[currentIndex].GunInfo.range;
        maxAmmo = gunList[currentIndex].GunInfo.maxAmmo;
        reloadDelay = gunList[currentIndex].GunInfo.reloadDelay;
        firerate = gunList[currentIndex].GunInfo.firerate;

        ammoCount = gunList[currentIndex].CurrentAmmoCount;
    }

    void SwitchGun()
    {
        if (gunList.Count < 2)
        {
            return;
        }

        switch (currentIndex)
        {
            case 0:

                if (!activateIndex[1])
                {
                    break;
                }

                currentIndex = 1;
                break;

            case 1:
                if (!activateIndex[0])
                {
                    break;
                }
                currentIndex = 0;
                break;
        }

        InitGun();
        prevIndex = currentIndex;

        if (ammoCount <= 0)
        {
            canShoot = false;
        }
    }

    public void ActivateGun(int index, bool value)
    {
        activateIndex[index] = value;
    }
}