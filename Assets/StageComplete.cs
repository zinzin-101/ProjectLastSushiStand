using SlimUI.ModernMenu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageComplete : MonoBehaviour
{
    [SerializeField] GameObject WinPanel;
    [SerializeField] TMP_Text timerText;

    private TimerScript timerScript;
    [SerializeField] UIMenuManager uiMenuManager;

    private void Awake()
    {
        
    }
    public void Active()
    {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void NextStage()
    {
        uiMenuManager.LoadSceneInt(SceneManager.GetActiveScene().buildIndex +1);
    }


}
