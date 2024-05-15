using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] float lifespan = 0.2f;
    [SerializeField] int meleeDamage = 10;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    //[SerializeField] float knockbackForce = 15f;

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out EnemyHp enemyHp))
        {
            print("collided");
            enemyHp.TakeDamage(meleeDamage);
            Instantiate(ImpactParticleSystem, enemyHp.transform.position, Quaternion.identity);

            //col.gameObject.TryGetComponent(out Rigidbody rb);
            //if (rb != null)
            //{
            //    Vector3 direction = rb.position - transform.position;
            //    rb.AddForce(direction.normalized * knockbackForce);
            //}
        }
    }
}
