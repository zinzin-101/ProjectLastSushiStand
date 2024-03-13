using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("scene_1");
    }
    public void PlayFirstPlay()
    {
        SceneManager.LoadScene("Map 1");
    }
    public void PlayPrototype()
    {
        SceneManager.LoadScene("scene_1");
    }
}
