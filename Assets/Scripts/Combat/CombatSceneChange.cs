using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CombatSceneChange : MonoBehaviour
{
    public static CombatSceneChange Instance;
    public int enemyAmount
    {
        get => m_enemyAmount;
        set
        {
            //If there are no enemies, the combat ends
            if (value == 0)
            {
                EndCombat();
            }
            m_enemyAmount = value;
        }
    }
    private int m_enemyAmount;

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
    }
    private void Start()
    {
        StartCoroutine(SpawnArena());
    }

    private IEnumerator SpawnArena()
    {
        while (true)
        {
            //Waits until the combat scene is loaded (important for spawning the arenas in the correct scene)
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Combat"))
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        NewDungeonGridGenerator tmp = NewDungeonGridGenerator.Instance;
        switch (tmp.CurrentEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                Instantiate(tmp.EasyEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case EEncounterType.FIGHTMEDIUM:
                Instantiate(tmp.MediumEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case EEncounterType.FIGHTHARD:
                Instantiate(tmp.HardEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case EEncounterType.BOSS:
                break;
            case EEncounterType.HEAL:
                break;
            default:
                break;
        }
        NavmeshBake.Instance.Bake();
    }

    public void AddEnemy()
    {
        enemyAmount++;
    }

    public void RemoveEnemy()
    {
        enemyAmount--;
    }

    public void EndCombat()
    {
        //Upon the comabt ending, the gates open
        AkSoundEngine.PostEvent("Play_OpenGate", this.gameObject);
        foreach (var _exit in FindObjectsOfType<PlayerExitZone>())
        {
            _exit.IsAbleToLeave = true;
        }
    }
}
