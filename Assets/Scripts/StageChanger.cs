using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    private SceneIndexManager sceneIndexManager;
    private GameManager gameManager;

    private void Awake()
    {
        sceneIndexManager = FindObjectOfType<SceneIndexManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            gameManager.Win = true;
            int lastSceneIndex = sceneIndexManager.GetLastSceneIndex();
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
            sceneIndexManager.SetLastSceneIndex(nextSceneIndex);

            
        }
    }
}
