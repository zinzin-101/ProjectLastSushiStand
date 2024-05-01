using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartController : MonoBehaviour
{

    [SerializeField] GameObject RestartPanel;
    [SerializeField] GameObject crosshair;

    public void Active()
    {
        Time.timeScale = 0.0f;
        RestartPanel.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Instance.SetGamePause(true);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
}
