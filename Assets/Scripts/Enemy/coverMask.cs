using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class coverMask : MonoBehaviour
    {
        public bool occupied = false;
        private GameObject enemy;
        private EnemyBrain cover;
        

        private void Awake()
        {
            
        }

        private void OnTriggerEnter(Collider Collider)
        {           
            if((enemy == null) && (Collider.tag != "Player"))
            {
                occupied = true;
                enemy = Collider.gameObject;
                cover = Collider.GetComponent<EnemyBrain>();
                cover.cover123 = true;
            }
            
        }

        private void OnTriggerExit(Collider collision)
        {
            
            if((collision.gameObject == enemy) && (collision.tag != "Player"))
            {
                occupied = false;
                enemy = null;   
                cover.cover123 = false;
                cover = null;
            }
        }

    }

}

