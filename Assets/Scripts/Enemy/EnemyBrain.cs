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
        [SerializeField] private GameObject player;
        private EnemyRef enemyRef;
        [SerializeField] private float shootingDistance = 8;
        [SerializeField] private float followDistance = 0;
        [SerializeField] private float viewDistance = 0;
        private bool seePlayer;
        private float pathUpdataDeadline;
        private EnemyShooter enemyShooter;
        private Cover cover;
        private Transform targetCover;
        [SerializeField] private float coverDistance;
        [SerializeField] private bool useCover = false;
        private float onCover;
        private Transform coverPos;
        [HideInInspector] public Vector3 enemyDirection;
        private Vector3 coverMask;
        private EnemyDirection direction;

        private ListCover coverList;
        private Cover ThisCover;
        private bool inRange;

        private Vector3 enemyPosition;
        [HideInInspector] public bool selectCover;
        [SerializeField] private Transform center;
        [SerializeField] private float maxFollowTime;
        private float followTime;


        private void Awake()
        {
            enemyRef = GetComponent<EnemyRef>();
            enemyShooter = GetComponentInChildren<EnemyShooter>();
            cover = FindObjectOfType<Cover>();
            coverList = FindAnyObjectByType<ListCover>();
            //player = GameObject.FindWithTag("Player");
            direction = GetComponentInChildren<EnemyDirection>();
        }

        private void Start()
        {
            shootingDistance = 8;          
        }

        private void FixedUpdate()
        {
            if (isPlayerNear() || seePlayer)
            {
                inRange = Vector3.Distance(transform.position, player.transform.position) <= followDistance;
                if (inRange)
                {
                    followTime = maxFollowTime;
                }

                followTime -= Time.deltaTime;
                if (player != null && cover != null && coverList != null)
                {
                    if (selectCover == false)
                    {
                        SelectCover();
                    }

                    inRange = Vector3.Distance(transform.position, player.transform.position) <= shootingDistance;
                    //Debug.Log("Distance" + Vector3.Distance(transform.position, player.transform.position));
                    if (inRange)
                    {

                        if ((coverDistance <= shootingDistance) && (useCover == true))
                        {
                            enemyRef.agent.stoppingDistance = 0;
                            GoToCover();
                        }
                        else
                        {
                            direction.LookAtTarget();
                            enemyShooter.Shoot();
                        }

                    }
                    else
                    {
                        selectCover = false;
                        UpdatPath();
                    }
                }
                else
                {
                    UpdatPath();
                    direction.LookAtTarget();
                    if (inRange = Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
                    {
                        enemyShooter.Shoot();
                    }
                }
            }
            
            
        }
        


        private void UpdatPath()
        {
            
            if(Time.time >= pathUpdataDeadline)
            {
                    pathUpdataDeadline = Time.time + enemyRef.pathUpdateDelay;
                    enemyRef.agent.stoppingDistance = 8;
                    enemyRef.agent.SetDestination(player.transform.position);
                
                Debug.Log("Path Updata");
                
            }
        }

        private void GoToCover()
        {
            if (coverDistance <= shootingDistance)
            {
                Debug.DrawRay(player.transform.position, -enemyDirection * 10f, Color.green);
                
                Debug.Log(targetCover.name);
                enemyRef.agent.SetDestination(targetCover.position);
                onCover = Vector3.Distance(enemyPosition, targetCover.position);
                if (onCover <= 1)
                {
                    
                    enemyRef.agent.SetDestination(targetCover.position);
                    direction.LookAtTarget();
                    enemyShooter.Shoot();
                }
            }
        }

        private void SelectCover()
        {
            ThisCover = coverList.UpdataNearestCover(player.transform.position);
            coverPos = ThisCover.transform;
            cover = ThisCover;
            Debug.Log(coverPos.name);
            
            enemyDirection = player.transform.position - coverPos.position;
            enemyDirection.Normalize();
            coverMask = (enemyDirection * -1) + coverPos.position;

            enemyPosition = transform.position;
            targetCover = cover.UpdataNearestCover(coverMask);
            Debug.Log("enemybrain" + targetCover.name);
            coverDistance = Vector3.Distance(targetCover.position, player.transform.position);
        }

        private bool isPlayerNear()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= viewDistance)
            { 
                followTime = maxFollowTime;
                seePlayer = true;
                return true;
    
            }
            if (followTime > 0)
            {
                return true;
            }

            enemyRef.agent.SetDestination(center.position);
            seePlayer = false;
            return false;
            

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(coverMask, 1);
        }
    }
}
