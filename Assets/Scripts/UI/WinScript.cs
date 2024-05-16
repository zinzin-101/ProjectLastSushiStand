using System.Collections;
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

    [SerializeField] float countingSpeed = 10.0f; // Speed at which the counter updates

    private void Start()
    {
        timerScript = FindObjectOfType<TimerScript>();
    }

    public void Active()
    {
        Time.timeScale = 0.0f;
        RestartPanel.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(UpdateTimerText(timerScript.ExportTime));

        GameManager.Instance.SetGamePause(true);
    }

    private IEnumerator UpdateTimerText(float targetTime)
    {
        float currentTime = 0.0f;

        while (currentTime < targetTime)
        {
            currentTime += Time.unscaledDeltaTime * countingSpeed;
            int minute = Mathf.FloorToInt(currentTime / 60);
            int second = Mathf.FloorToInt(currentTime % 60);
            timerText.text = "Time Taken: " + string.Format("{0:00}:{1:00}", minute, second);
            yield return null;
        }

        // Ensure the timer displays the exact ExportTime after the loop completes
        int finalMinute = Mathf.FloorToInt(targetTime / 60);
        int finalSecond = Mathf.FloorToInt(targetTime % 60);
        timerText.text = "Time Taken: " + string.Format("{0:00}:{1:00}", finalMinute, finalSecond);
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
