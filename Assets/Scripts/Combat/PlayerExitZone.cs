using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExitZone : MonoBehaviour
{
    public bool IsAbleToLeave;
    public bool IsExit;
    //public static PlayerExitZone Instance;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        IsAbleToLeave = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsAbleToLeave)
        {
            MapManager.Instance.ChangeMapState(true);
            SceneManager.UnloadSceneAsync("Combat");
            MapManager.Instance.PlayerTookExit = IsExit;
        }
    }
}
