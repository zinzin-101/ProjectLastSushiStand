using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class DroneBrain : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Transform shootPoint;
        private EnemyShooter shooter;
        [SerializeField] private Transform center;
        [SerializeField] private int RandomposDelay = 1;
        private Vector3 targetPos;
        [SerializeField] private int shootingDistance = 8;
        private EnemyDirection direction;
        [SerializeField] private float speed = 0.1f;
        
        private void Awake()
        {
            shooter = GetComponent<EnemyShooter>();
            player = GameObject.FindWithTag("Player");
            direction = GetComponentInChildren<EnemyDirection>();
        }

        private void Update()
        {
            direction.LookAtTarget();
            if (Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
            {
                shooter.Shoot();
            }


            if (RandomposDelay == 1)
            {
                Debug.Log("RandomPos");
                RandomPos();
                RandomposDelay = 0;
            }
            if (targetPos != null)
            {
                Move();
            }

            if (transform.position == targetPos)
            {
                RandomposDelay = 1;
            }
        }


        private void RandomPos()
        {
            float x = Random.Range(-2.0f, 2.0f);
            float z = Random.Range(-2.0f, 2.0f);
            targetPos = new Vector3(center.position.x + x, center.position.y, center.position.z + z);
            Debug.Log(targetPos);
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
        }

        //debugZone
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(center.position, new Vector3(4, 0, 4));
        }
    }
}


