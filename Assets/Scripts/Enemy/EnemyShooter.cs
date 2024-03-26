using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{

    public class EnemyShooter : MonoBehaviour
    {
        [Header("General")]
        [SerializeField ]private Transform shootPoint;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private LayerMask layerMask;

        [Header("Gun")]
        [SerializeField] private Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);
        [SerializeField] private TrailRenderer bulletTrail;
      
        [SerializeField] private int bullet = 1;
        [SerializeField] private float reloadtime = 2;
        private float reloadTime = 0;

        [SerializeField] private PlayerStatus PlayerHp;

        public void Awake()
        {

        }

        public void Shoot()
        {
            if(bullet != 0) 
            {
                
                Vector3 direction = GetDirection();
                
                if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
                {
                    Debug.Log("Shoot");
                    Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 10f, Color.red, 1f);
                    
                    TrailRenderer trail = Instantiate(bulletTrail, gunPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                    bullet--;
                    reloadTime = reloadtime;
                }
            }
            else
            {
                
                if(reloadTime > 0)
                {
                    reloadTime -= Time.deltaTime;
                }
                else
                {
                    
                    bullet = 1;
                }
            }
            
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = gunPoint.transform.forward;
            direction += new Vector3(
                Random.Range(-spread.x, spread.x),
                Random.Range(-spread.y, spread.y),
                Random.Range(-spread.z, spread.z)); 
            direction.Normalize();
            return direction;
        }

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0f;
            Vector3 startPosition = trail.transform.position;

            while(time  < 1f)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;
                yield return null;
            }

            trail.transform.position = hit.point;
            Destroy(trail.gameObject, trail.time);  
        }
    }
}
