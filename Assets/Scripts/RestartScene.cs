using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RestartScene : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent(out PlayerStatus playerStatus))
        {
            playerStatus.SetPlayerAlive(false);

        }
    }

}
