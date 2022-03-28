using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CombatSceneChange : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.transform.position = new Vector3(0, 2, 0);
    }
    public void Interact(InputAction.CallbackContext context)
    {
        //EndCombat();
    }

    public void EndCombat()
    {
        HideMap.Instance.ChangeMapState();
        SceneManager.UnloadSceneAsync("Combat");
    }
}
