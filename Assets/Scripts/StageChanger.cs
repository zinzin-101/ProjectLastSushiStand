using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    private SceneIndexManager sceneIndexManager;

    private bool win = false;

    public bool Win => win;

    private void Awake()
    {
        sceneIndexManager = FindObjectOfType<SceneIndexManager>();
        win = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            win = true;

            /*
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
            */
        }
    }
}
