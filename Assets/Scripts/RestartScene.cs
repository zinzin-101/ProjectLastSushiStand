using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RestartScene : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("test1");
        if (col.gameObject.TryGetComponent(out PlayerMovement plrScript))
        {
            Debug.Log("test");
            SceneManager.LoadScene("Map 1");
        }
    }

}
