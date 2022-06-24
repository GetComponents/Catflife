using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EncounterCell : MonoBehaviour
{
    public List<EncounterCell> NextCells = new List<EncounterCell>();
    public bool ContainsEncounter;
    public bool IsConnected = false, IsDeadEnd = true, IsCleared;
    public int MyEncounterIndex;
    [SerializeField]
    GameObject StartEncounterUI;

    public EEncounterType MyEncounter
    {
        get => m_myEncounter;
        set
        {
            switch (value)
            {
                case EEncounterType.NONE:
                    break;
                case EEncounterType.FIGHTEASY:
                    GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case EEncounterType.FIGHTMEDIUM:
                    GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case EEncounterType.FIGHTHARD:
                    GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case EEncounterType.BOSS:
                    break;
                case EEncounterType.HEAL:
                    break;
                default:
                    break;
            }
            m_myEncounter = value;
        }
    }
    [SerializeField]
    private EEncounterType m_myEncounter;
    private bool wasClicked;

    public Transform LeftWallPos, RightWallPos, EntrancePos, ExitPos;


    public void StartEncounter()
    {
        Debug.Log("CLickable = " + wasClicked);
        if (wasClicked == false)
        {
            wasClicked = true;
            GenerateEncounter();
        }
    }

    public void GenerateEncounter()
    {
        switch (MyEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                break;
            case EEncounterType.FIGHTMEDIUM:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                break;
            case EEncounterType.FIGHTHARD:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                break;
            case EEncounterType.BOSS:
                if (true)
                {
                    SceneTransition.Instance.ChangeScene("BossStage", 0);
                }
                else
                {
                    //In case the Player hasnt unlocked the StoryItem to fight the boss
                    PlayerController.Instance.Die();
                }
                break;
            case EEncounterType.HEAL:
                break;
            default:
                break;
        }
        NewDungeonGridGenerator.Instance.CurrentEncounter = MyEncounter;
        NewDungeonGridGenerator.Instance.CurrentEncounterIndex = MyEncounterIndex;
        NewDungeonGridGenerator.Instance.ClearedArena = IsCleared;
        MapManager.Instance.CurrentCell = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            StartEncounterUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            StartEncounterUI.SetActive(false);
    }
}

public enum EEncounterType
{
    NONE,
    FIGHTEASY,
    FIGHTMEDIUM,
    FIGHTHARD,
    BOSS,
    HEAL
}
