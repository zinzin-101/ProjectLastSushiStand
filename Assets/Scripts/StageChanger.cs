using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            if (SceneManager.GetActiveScene().buildIndex < 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("Main Menu");
            }
            
        }
    }
}
