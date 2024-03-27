using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    public class EnemyHp : MonoBehaviour
    {
        [SerializeField] private int HP = 3;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(HP <= 0) {
                Destroy(gameObject);
            }
        }

        //private void OnTriggerEnter(Collider collision)
        //{
        //    if (collision.tag == "Bullet")
        //    {
        //        HP--;
        //    }
        //}

        public void TakeDamage(int damage)
        {
            HP -= damage;
        }
    }
}
