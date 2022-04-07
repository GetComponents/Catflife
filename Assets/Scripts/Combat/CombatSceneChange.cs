using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CombatSceneChange : MonoBehaviour
{
    public List<GameObject> AllArenaPrefabs;

    private void Awake()
    {
    }
    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Combat"));
        Instantiate(AllArenaPrefabs[Random.Range(0, AllArenaPrefabs.Count - 1)], transform.position, Quaternion.identity);
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
