using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.EnemyCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
