using Enemy;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ShootingSystem;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private float BulletSpeed = 100;


    private float LastShootTime;

    [SerializeField] private int damage = 1;

    [SerializeField] Camera camera;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }
    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {

            ShootingSystem.Play();
            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                Quaternion trailRotation = Quaternion.LookRotation(direction);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, trailRotation));

                LastShootTime = Time.time;
            }
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                Quaternion trailRotation = Quaternion.LookRotation(direction);

                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, Vector3.zero, false, trailRotation));

                LastShootTime = Time.time;
            }
            if (hit.collider.gameObject.TryGetComponent(out EnemyHp enemyHp))
            {
                enemyHp.TakeDamage(damage);
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = BulletSpawnPoint.transform.forward;
        direction = BulletSpawnPoint.transform.up;
        direction = BulletSpawnPoint.transform.right;
        direction.Normalize();
        if (AddBulletSpread)
        {
            direction += transform.up * Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y);
            direction += transform.right * Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x);
            direction += transform.forward * Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z);

            direction.Normalize();
        }
        
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, Quaternion rotation)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;
        Trail.transform.rotation = rotation;
        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}