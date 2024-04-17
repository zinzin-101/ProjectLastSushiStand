using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
    
    public class HeadEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyHp enemyHp;
        [SerializeField] private int critDamage = 2;

        public void Headshot(int damage)
        {
            //enemyHp.TakeDamage(damage * critDamage);
            enemyHp.TakeDamage(damage);
        }
    }
}