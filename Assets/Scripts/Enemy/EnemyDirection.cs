using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    public class EnemyDirection : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void Awake()
        {
            //player = GameObject.FindWithTag("Player");
        }

        public void LookAtTarget()
        {
            Vector3 lookPos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
            Debug.DrawLine(transform.position, player.transform.position, Color.blue, 0f);
        }
    }
}
