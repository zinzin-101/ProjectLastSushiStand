using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float lifeTime = 5;
    private Vector3 origin;
    public Vector3 Origin => origin;

    private void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed * 10;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
           other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
           other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("hit");
            Destroy(gameObject);
        }
    }
}
