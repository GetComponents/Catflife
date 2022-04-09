using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CombatSceneChange : MonoBehaviour
{
    public static CombatSceneChange Instance;
    private int enemyAmount
    {
        get => m_enemyAmount;
        set
        {
            if (value < m_enemyAmount && value == 0)
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Combat"));
        SpawnArena();
        //Instantiate(AllArenaPrefabs[Random.Range(0, AllArenaPrefabs.Count - 1)], transform.position, Quaternion.identity);
    }

    private void SpawnArena()
    {
        DungeonGridGenerator tmp = DungeonGridGenerator.Instance;
        switch (DungeonGridGenerator.Instance.CurrentEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                Instantiate(tmp.EasyEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.identity);
                break;
            case EEncounterType.FIGHTMEDIUM:
                Instantiate(tmp.MediumEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.identity);
                break;
            case EEncounterType.FIGHTHARD:
                Instantiate(tmp.HardEncounters[tmp.CurrentEncounterIndex], transform.position, Quaternion.identity);
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
        PlayerExitZone.Instance.IsAbleToLeave = true;
    }
}
