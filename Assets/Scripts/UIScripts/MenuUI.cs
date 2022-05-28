using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        SceneManager.LoadScene("MainRoom");
    }

    public void Continue()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.UnloadSceneAsync("PauseScreen");
    }

    public void StartMenu()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("StartScreen");
    }

    public void OpenControls()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        SceneManager.LoadScene("ControlsScreenPause", LoadSceneMode.Additive);
    }

    public void CloseControls()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        SceneManager.UnloadSceneAsync("ControlsScreenPause");
    }

    public void OpenCredits()
    {
        AkSoundEngine.PostEvent("Play_ButtonPress", this.gameObject);
        SceneManager.LoadScene("CreditsScreen", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
