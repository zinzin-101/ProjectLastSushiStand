
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

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
        [SerializeField] private int speedMovement = 8;
        bool check = true;


        private void Awake()
        {
            enemyRef = GetComponent<EnemyRef>();
            enemyShooter = GetComponentInChildren<EnemyShooter>();
            cover = FindObjectOfType<Cover>();
            coverList = FindAnyObjectByType<ListCover>();
            PlayerMovement playerScript = FindFirstObjectByType<PlayerMovement>();
            player = playerScript.gameObject;
            direction = GetComponentInChildren<EnemyDirection>();
        }

        private void Start()
        {
            enemyRef.agent.speed = speedMovement;
        }

        private void FixedUpdate()
        {
            if (isPlayerNear())
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
                    //enemyRef.agent.stoppingDistance = shootingDistance;
                    UpdatPath();
                    direction.LookAtTarget();
                    if (inRange = Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
                    {
                        enemyShooter.Shoot();
                    }
                }
            }
            else
            {
                UpdatPath();
            }

            Debug.Log(enemyRef.agent.pathEndPosition);
        }
        


        private void UpdatPath()
        {

            if(Time.time >= pathUpdataDeadline)
            {
                
                if (isPlayerNear())
                {
                    if (ramdomNext() && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance && check)
                    {
                        float x = Random.Range(-2.0f, 2.0f);
                        float z = Random.Range(-2.0f, 2.0f);
                        enemyRef.agent.stoppingDistance = 0;
                        Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
                        Debug.Log("Enemy Pos" + transform.position);
                        Debug.Log("target RandomPos" + pos);
                        enemyRef.agent.SetDestination(pos);
                        check = false;
                    }

                    else if(check)
                    {
                        pathUpdataDeadline = Time.time + enemyRef.pathUpdateDelay;
                        enemyRef.agent.SetDestination(player.transform.position);
                        Debug.Log("target Player");
                    }
                }

                else
                { 
                    enemyRef.agent.stoppingDistance = 0;
                    enemyRef.agent.SetDestination(center.position);
                    check = true;
                    Debug.Log("target Center");
                }

                
                
                
                //Debug.Log("Path Updata");
                //Debug.Log(enemyRef.agent.destination);
            }
            if (Vector3.Distance(transform.position, enemyRef.agent.pathEndPosition) <= 1.5f)
            {
                Debug.Log("New End Pos");
                check = true;
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
                seePlayer = true;
                return true;
                
            }

            seePlayer = false;
            return false;
            

        }

        private bool isSeePlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            if(Mathf.Abs(Vector3.Angle(transform.forward, dir)) < 65)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ramdomNext()
        {
            int random = Random.Range(0, 100);
            
            if (random >= 50)
            {
                    return true;
            }

            return false;
        }



        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(coverMask, 1);
        }
    }
}
