using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map4Blocker : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public bool isCollide = false;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.EnemyCount <= 11)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement plrScripts))
        {
            isCollide = true;
        }
    }
}
