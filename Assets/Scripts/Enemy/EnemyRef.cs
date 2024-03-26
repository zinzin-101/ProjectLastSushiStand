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
        [SerializeField] private AnimationCurve m_Curve = new AnimationCurve();
        private void Awake()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
        }
    }

    
}
