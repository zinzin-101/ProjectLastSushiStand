using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayFirstPlay()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void PlayStageOne()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayStageTwo()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayStageThree()
    {
        SceneManager.LoadScene(3);
    }
    public void PlayTest()
    {
        SceneManager.LoadScene("Sandbox");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
