using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        //PlaySound ButtonPress
        SceneManager.LoadScene("MainRoom");
    }

    public void Continue()
    {
        //PlaySound ButtonPress
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.UnloadSceneAsync("PauseScreen");
    }

    public void StartMenu()
    {
        //PlaySound ButtonPress
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
