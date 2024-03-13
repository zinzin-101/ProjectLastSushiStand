using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Map 1");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
