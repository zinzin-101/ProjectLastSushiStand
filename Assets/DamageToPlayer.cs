using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    [SerializeField] PlayerStatus PlayerHp;

    private void Awake()
    {
        PlayerHp = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider Collider)
    {
        if ((Collider.tag == "Player"))
        {
            PlayerHp.TakeDamage(5);
        }

    }
}
