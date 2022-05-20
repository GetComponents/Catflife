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
            if (value == 0)
            {
                Debug.Log("The Gates Open");
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
        SpawnArena();
    }

    private void SpawnArena()
    {
        NewDungeonGridGenerator tmp = NewDungeonGridGenerator.Instance;
        switch (tmp.CurrentEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                Instantiate(tmp.EasyEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, -90f, 0));
                break;
            case EEncounterType.FIGHTMEDIUM:
                Instantiate(tmp.MediumEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, -90f, 0));
                break;
            case EEncounterType.FIGHTHARD:
                Instantiate(tmp.HardEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.Euler(0, -90f, 0));
                break;
            case EEncounterType.BOSS:
                break;
            case EEncounterType.HEAL:
                break;
            default:
                break;
        }
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
        AkSoundEngine.PostEvent("Play_DoorSqueaky", this.gameObject);
        foreach (var _exit in FindObjectsOfType<PlayerExitZone>())
        {
            _exit.IsAbleToLeave = true;
        }
    }
}
