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
    public bool Clickable;

    public Transform LeftWallPos, RightWallPos, EntrancePos, ExitPos;


    public void StartEncounter()
    {
        if (Clickable == true)
        {
            //DisableOtherCells();
            GenerateEncounter();
        }
    }
    private void DisableOtherCells()
    {
        //foreach (EncounterCell cell in DungeonGridGenerator.Instance.SelectableCells)
        //{
        //    cell.Clickable = false;
        //}
        //DungeonGridGenerator.Instance.SelectableCells.Clear();
        //foreach (EncounterCell cell in NextCells)
        //{
        //    cell.Clickable = true;
        //    DungeonGridGenerator.Instance.SelectableCells.Add(cell);
        //}
        //DungeonGridGenerator.Instance.MoveMap(gameObject.transform.localScale.y);
        //MapManager.Instance.ChangeMapState(false);
    }

    public void GenerateEncounter()
    {
        switch (MyEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                //SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.FIGHTMEDIUM:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                //SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.FIGHTHARD:
                SceneTransition.Instance.ChangeScene("Combat", 1);
                //SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.BOSS:
                if (true)
                {
                    SceneTransition.Instance.ChangeScene("BossStage", 0);
                }
                else
                {
                    PlayerController.Instance.Die();
                }
                break;
            case EEncounterType.HEAL:
                break;
            default:
                break;
        }
        //SceneTransition.Instance?.StartTransition();
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
