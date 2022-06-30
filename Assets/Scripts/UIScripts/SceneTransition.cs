using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    [SerializeField]
    Image TransitionImage;

    [SerializeField]
    SkinnedMeshRenderer playerSword;

    [SerializeField]
    GameObject dontDestroyCanvas;

    string sceneToLoad;
    int loadType;

    public string SceneToLoad => sceneToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.activeSceneChanged += EndTransition;
        DontDestroyOnLoad(dontDestroyCanvas);
    }

    /// <summary>
    /// Changes the current scene
    /// </summary>
    /// <param name="_sceneToLoad"></param>
    /// <param name="_loadType">0:normal, 1:addative, 2:unload combat</param>
    public void ChangeScene(string _sceneToLoad, int _loadType)
    {
        sceneToLoad = _sceneToLoad;
        loadType = _loadType;
        SetSceneSounds(sceneToLoad);
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
        var tempColor = TransitionImage.color;
        for (float i = 0; i <= 1; i += 0.1f)
        {
            tempColor.a = i;
            TransitionImage.color = tempColor;
            yield return new WaitForSeconds(0.1f);
        }
        switch (loadType)
        {
            case 0:
                SceneManager.LoadScene(sceneToLoad);
                break;
            case 1:
                SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
                while (true)
                {
                    if (SceneManager.GetSceneByName(sceneToLoad).isLoaded)
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
                MapManager.Instance.ChangeMapState(false);

                break;
            case 2:
                SceneManager.UnloadSceneAsync(sceneToLoad);
                while (true)
                {
                    if (SceneManager.GetActiveScene().name == "EncounterSelection")
                    {
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
                MapManager.Instance.ChangeMapState(true);
                break;
            default:
                break;
        }

    }

    //Manages logic that happens depending on what scene was/is loaded
    public void EndTransition(Scene _previousScene, Scene _newScene)
    {
        string previousSceneName = _previousScene.name;
        string newSceneName = _newScene.name;
        if (newSceneName == "Combat" || "CombatDebug" == newSceneName || newSceneName == "BossStage")
        {
            PlayerController.Instance.IsInCombat = true;
            playerSword.enabled = true;
        }
        else if (previousSceneName == "Combat")
        {
            PlayerController.Instance.IsInCombat = false;
            playerSword.enabled = false;
        }
        if (newSceneName == "EndScene")
        {
           
        }
        if(newSceneName == "MainRoom")
        {
            PlayerController.Instance.Revive();
            PlayerController.Instance.IsInCombat = false;
            playerSword.enabled = false;
        }
        StartCoroutine(FinishTransition());
    }

    private IEnumerator FinishTransition()
    {
        var tempColor = TransitionImage.color;
        for (float i = 1; i > 0; i -= 0.1f)
        {
            tempColor.a = i;
            TransitionImage.color = tempColor;
            yield return new WaitForSeconds(0.1f);
        }
        tempColor.a = 0;
        TransitionImage.color = tempColor;
    }
    
    private void SetSceneSounds(string sceneToLoad)
    {
        if (sceneToLoad.Equals("MainRoom"))
        {
            AkSoundEngine.PostEvent("Stop_DungeonAmb", this.gameObject);
            AkSoundEngine.PostEvent("Play_HouseAmb", this.gameObject);

            AkSoundEngine.SetSwitch("Material", "Default", PlayerController.Instance._akGameObj.gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Stop_HouseAmb", this.gameObject);
            AkSoundEngine.PostEvent("Play_DungeonAmb", this.gameObject);
            AkSoundEngine.SetSwitch("Material", "Stone", PlayerController.Instance._akGameObj.gameObject);
        }
    }
}
