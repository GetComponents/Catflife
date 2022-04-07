using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExitZone : MonoBehaviour
{
    public bool IsAbleToLeave = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsAbleToLeave)
        {
            HideMap.Instance.ChangeMapState();
            SceneManager.UnloadSceneAsync("Combat");
        }
    }
}
