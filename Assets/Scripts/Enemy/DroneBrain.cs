using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class DroneBrain : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        public Transform shootPoint;
        private EnemyShooter shooter;
        [SerializeField] private Transform center;
        private int RandomposDelay = 1;
        private float speed = 0.01f;
        private Vector3 targetPos;
        private int shootingDistance = 5;
        private void Awake()
        {
            shooter = GetComponent<EnemyShooter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            LookAtTarget();
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

        private void LookAtTarget()
        {
            Vector3 lookPos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
            Debug.DrawLine(shootPoint.position, player.transform.position, Color.blue, 0f);
        }

        private void RandomPos()
        {
            float x = Random.Range(-2.0f, 2.0f);
            float z = Random.Range(-2.0f, 2.0f);
            targetPos = new Vector3(center.position.x + x, center.position.y, center.position.z + z);
            //Debug.Log(targetPos);
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


