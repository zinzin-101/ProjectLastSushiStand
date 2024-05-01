using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    [SerializeField] int damage = 1;
    //[SerializeField] PlayerStatus PlayerHp;

    private void Awake()
    {
        //PlayerHp = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out PlayerStatus PlayerHp))
        {
            PlayerHp.TakeDamage(damage);
        }

    }
}
