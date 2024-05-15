using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private bool isPaused;
    public bool IsPaused => isPaused;

    private int enemyCount;
    public int EnemyCount => enemyCount;

    private bool enemiesExist;
    public bool EnemiesExist => enemiesExist;

    public bool Win = false;

    private void Awake()
    {
        SoundManager.Initialize();
        Win = false;
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        Time.timeScale = 1.0f;

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            SoundManager.PlaySound(SoundManager.Sound.bgm2);
        }else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            SoundManager.PlaySound(SoundManager.Sound.bgm2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SoundManager.PlaySound(SoundManager.Sound.bgm2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SoundManager.PlaySound(SoundManager.Sound.bgm2);
        }
    }

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        UpdateEnemyCount(); // more reliable but computationally expensive to put in Update()
        //print(enemyCount);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetGamePause(bool state)
    {
        isPaused = state;
    }

    public void UpdateEnemyCount()
    {
        enemyCount = GameObject.FindObjectsOfType<EnemyHp>().Length;

        if (enemyCount <= 0)
        {
            enemiesExist = false;
        }
        else
        {
            enemiesExist = true;
        }
    }
}