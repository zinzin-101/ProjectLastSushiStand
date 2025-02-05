using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    public class EnemyHp : MonoBehaviour
    {
        [SerializeField] private int HP = 10;
        public int MaxHp => HP;
        [SerializeField] private GameObject enemy;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(HP <= 0) {
                GameManager.Instance.UpdateEnemyCount();
                SoundManager.PlaySound(SoundManager.Sound.EnemyDead);
                Destroy(enemy);
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
