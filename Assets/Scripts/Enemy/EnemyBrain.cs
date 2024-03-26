using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

namespace Enemy
{
    public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private EnemyRef enemyRef;
        private float shootingDistance;
        private float pathUpdataDeadline;
        private EnemyShooter enemyShooter;
        private Cover cover;
        private Transform targetCover;
        private float coverDistance;
        [SerializeField] private bool useCover = false;
        private float onCover;
        private Transform coverPos;
        [HideInInspector] public Vector3 enemyDirection;
        private Vector3 coverMask;

        private ListCover coverList;
        private Cover ThisCover;

        private Vector3 enemyPosition;
        public bool cover123;



        private void Awake()
        {
            enemyRef = GetComponent<EnemyRef>();
            enemyShooter = GetComponent<EnemyShooter>();
            cover = FindObjectOfType<Cover>();
            coverList = FindAnyObjectByType<ListCover>();

        }

        private void Start()
        {
            shootingDistance = 8;          
        }

        private void FixedUpdate()
        {
            if (player != null)
            {
                if(cover123 == false)
                {
                    SelectCover();
                }
                
                bool inRange = Vector3.Distance(transform.position, player.position) <= shootingDistance;
                if (inRange)
                {

                    if ((coverDistance <= shootingDistance) && (useCover == true))
                    {
                        enemyRef.agent.stoppingDistance = 0;
                        GoToCover();
                       
                    }
                    else
                    {
                        LookAtTarget();
                        enemyShooter.Shoot();
                    }

                }
                else
                {
                    cover123 = false;
                    UpdatPath();
                }
            }
            
        }
        

        private void LookAtTarget()
        {
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

        private void UpdatPath()
        {
            
            if(Time.time >= pathUpdataDeadline)
            {
                    pathUpdataDeadline = Time.time + enemyRef.pathUpdateDelay;
                    enemyRef.agent.stoppingDistance = 8;
                    enemyRef.agent.SetDestination(player.position);
                
                Debug.Log("Path Updata");
                
            }
        }

        private void GoToCover()
        {
            if (coverDistance <= shootingDistance)
            {
                Debug.DrawRay(player.position, -enemyDirection * 10f, Color.green);
                
                Debug.Log(targetCover.name);
                enemyRef.agent.SetDestination(targetCover.position);
                onCover = Vector3.Distance(enemyPosition, targetCover.position);
                if (onCover <= 1)
                {
                    
                    enemyRef.agent.SetDestination(targetCover.position);
                    LookAtTarget();
                    enemyShooter.Shoot();
                }
            }
        }

        private void SelectCover()
        {
            ThisCover = coverList.UpdataNearestCover(player.position);
            coverPos = ThisCover.transform;
            cover = ThisCover;
            Debug.Log(coverPos.name);
            
            enemyDirection = player.position - coverPos.position;
            enemyDirection.Normalize();
            coverMask = (enemyDirection * -1) + coverPos.position;

            enemyPosition = transform.position;
            targetCover = cover.UpdataNearestCover(coverMask);
            Debug.Log("enemybrain" + targetCover.name);
            coverDistance = Vector3.Distance(targetCover.position, player.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(coverMask, 1);
        }
    }
}
