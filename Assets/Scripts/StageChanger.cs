using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    private SceneIndexManager sceneIndexManager;

    private void Awake()
    {
        sceneIndexManager = FindObjectOfType<SceneIndexManager>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            int lastSceneIndex = sceneIndexManager.GetLastSceneIndex();
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
            sceneIndexManager.SetLastSceneIndex(nextSceneIndex);

            if (SceneManager.GetActiveScene().buildIndex < 3)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("NewMainMenu");
            }
        }
    }
}
