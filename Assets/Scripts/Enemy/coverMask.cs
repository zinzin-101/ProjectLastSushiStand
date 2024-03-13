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
            if((enemy == null) && (Collider.tag == "Enemy"))
            {
                occupied = true;
                enemy = Collider.gameObject;
                cover = Collider.GetComponent<EnemyBrain>();
                cover.selectCover = true;
            }
            
        }

        private void OnTriggerExit(Collider collision)
        {
            
            if((collision.gameObject == enemy) && (collision.tag != "Enemy"))
            {
                occupied = false;
                enemy = null;   
                cover.selectCover = false;
                cover = null;
            }
        }

    }

}

