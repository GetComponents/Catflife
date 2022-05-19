using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExitZone : MonoBehaviour
{
    public bool IsAbleToLeave;
    public bool IsExit;

    private void Awake()
    {
        IsAbleToLeave = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsAbleToLeave)
        {
            //PlaySound LeavingFootsteps
            MapManager.Instance.PlayerTookExit = IsExit;
            MapManager.Instance.ChangeMapState(true);
            SceneManager.UnloadSceneAsync("Combat");
        }
    }
}
