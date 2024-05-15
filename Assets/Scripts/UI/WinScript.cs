using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{

    [SerializeField] GameObject RestartPanel;
    [SerializeField] GameObject crosshair;

    [SerializeField] TimerScript timerScript;
    [SerializeField] TMP_Text timerText;

    private void Start()
    {
        timerScript = FindObjectOfType<TimerScript>();
        int minute = Mathf.FloorToInt(timerScript.CurrentTime / 60);
        int second = Mathf.FloorToInt(timerScript.CurrentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minute, second);

    }
    
    public void Active()
    {
        Time.timeScale = 0.0f;
        RestartPanel.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Instance.SetGamePause(true);
    }
    public void NextStage()
    {
        if (SceneManager.GetActiveScene().buildIndex < 4)
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
