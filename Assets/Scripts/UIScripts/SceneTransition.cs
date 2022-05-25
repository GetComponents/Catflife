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

    string sceneToLoad;
    int loadType;

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
                Debug.Log("trying to load " + sceneToLoad);
                break;
            case 1:
                Debug.Log("trying to load " + sceneToLoad + " additive");
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
                MapManager.Instance.ChangeMapState(true);
                break;
            default:
                break;
        }

    }

    public void EndTransition(Scene _previousScene, Scene _newScene)
    {
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
