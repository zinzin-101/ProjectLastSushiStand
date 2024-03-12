using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 10;
    public int MaxPlayerHealth => maxPlayerHealth;

    private int playerHealth;
    public int PlayerHealth => playerHealth;

    private void Start()
    {
        playerHealth = maxPlayerHealth;
    }

    private void Update()
    {
        if (playerHealth <= 0)
        {
            GameManager.Instance.ReloadCurrentScene();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //detect enemy bullet collision then remove health
        //or remove health from enemy bullet script
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
    }
}
