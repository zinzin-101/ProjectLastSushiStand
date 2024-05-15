using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private float currentTime;
    public float CurrentTime => currentTime;

    private bool timerActivate;

    private void Start()
    {
        timerActivate = true;
        currentTime = 0f;
    }

    private void Update()
    {
        if (timerActivate && !GameManager.Instance.IsPaused)
        {
            currentTime += Time.deltaTime;

            Debug.Log("Current Time: " + currentTime);
        }

        //print("time: " + currentTime);
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    public void SetActivateTimer(bool value)
    {
        timerActivate = value;
    }
}
