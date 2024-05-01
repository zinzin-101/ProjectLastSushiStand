using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject crosshair;

    public void Pause()
    {
        Time.timeScale = 0.0f;
        PausePanel.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Instance.SetGamePause(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.SetGamePause(false);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}