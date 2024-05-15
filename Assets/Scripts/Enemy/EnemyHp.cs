using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    public class EnemyHp : MonoBehaviour
    {
        [SerializeField] private int hp = 10;
        [SerializeField] ParticleSystem explosionFX;
        public int HP => hp;

        [SerializeField] private GameObject enemy;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(hp <= 0) {
                GameManager.Instance.UpdateEnemyCount();
                SoundManager.PlaySound(SoundManager.Sound.EnemyDead);

                Instantiate(explosionFX, transform.position, Quaternion.identity);

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
            hp -= damage;
        }
    }
}
