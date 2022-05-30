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

    public void ChangeScene(string _sceneToLoad, int _loadType)
    {
        sceneToLoad = _sceneToLoad;
        loadType = _loadType;
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

    public void EndTransition(Scene _previousScene, Scene _newScene)
    {
        string newSceneName = _newScene.name;
        Debug.Log("new scene name: " + newSceneName);
        if (newSceneName == "Combat" || "CombatDebug" == newSceneName || newSceneName == "BossStage")
        {
            PlayerController.Instance.IsInCombat = true;
            playerSword.enabled = true;
        }
        else if (_previousScene == SceneManager.GetSceneByName("Combat"))
        {
            PlayerController.Instance.IsInCombat = false;
            playerSword.enabled = false;
        }
        if (newSceneName == "EndScene")
        {
            //GameObject[] tmp = SceneManager.GetSceneByName("DontDestroyOnLoad").GetRootGameObjects();
            //Debug.Log("Objectamount in ddol: " + tmp.Length);
            //foreach (GameObject _object in tmp)
            //{
            //    if (_object == dontDestroyCanvas)
            //    {
            //        continue;
            //    }
            //    Debug.Log(_object.name);
            //    Destroy(_object);
            //}
            //Destroy(dontDestroyCanvas);
        }
        if(_newScene == SceneManager.GetSceneByName("MainRoom"))
        {
            PlayerController.Instance.Revive();
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
}
