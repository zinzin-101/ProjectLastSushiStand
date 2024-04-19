using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float lifeTime = 5;
    void Update()
    {
        lifeTime -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed * 500);
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
