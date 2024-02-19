using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DroneBrain : MonoBehaviour
    {
        private EnemyShooter enemyShooter;

        private void Awake()
        {
            enemyShooter = GetComponent<EnemyShooter>();
        }

        private void Update()
        {
            enemyShooter.Shoot();
        }
    }
}


