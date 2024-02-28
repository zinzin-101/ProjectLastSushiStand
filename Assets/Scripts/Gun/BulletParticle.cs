using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public float duration = 3;

    private void Awake()
    {
        Destroy(gameObject, duration);
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        //Destroy(collision.gameObject);
        //Destroy(gameObject);
    }
}
