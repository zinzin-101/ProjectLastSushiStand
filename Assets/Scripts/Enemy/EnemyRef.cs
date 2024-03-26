using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyRef : MonoBehaviour
    {
        public NavMeshAgent agent;
        public float pathUpdateDelay = 0.2f;
        private void Awake()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
        }
    }

    
}
