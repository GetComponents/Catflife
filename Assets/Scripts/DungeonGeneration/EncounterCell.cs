using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncounterCell : MonoBehaviour
{
    public List<EncounterCell> NextCells = new List<EncounterCell>();
    //public List<UILineRenderer> CellConnection = new List<UILineRenderer>();
    public bool IsConnected = false;
    public int MyEncounterIndex;
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
                    //GetComponent<Image>().sprite = easyEncounter;
                    GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case EEncounterType.FIGHTMEDIUM:
                    //GetComponent<Image>().sprite = mediumEncounter;
                    GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case EEncounterType.FIGHTHARD:
                    //GetComponent<Image>().sprite = hardEncounter;
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
    [SerializeField]
    Sprite easyEncounter, mediumEncounter, hardEncounter;


    public void StartEncounter()
    {
        if (Clickable == true)
        {
            MapManager.Instance.StagePos = transform.position;
            DisableOtherCells();
            GenerateEncounter();
        }
    }
    private void DisableOtherCells()
    {
        foreach (EncounterCell cell in DungeonGridGenerator.Instance.SelectableCells)
        {
            //cell.gameObject.GetComponent<Image>().color = Color.red;
            cell.Clickable = false;
        }
        DungeonGridGenerator.Instance.SelectableCells.Clear();
        foreach (EncounterCell cell in NextCells)
        {
            //cell.gameObject.GetComponent<Image>().color = Color.white;
            cell.Clickable = true;
            DungeonGridGenerator.Instance.SelectableCells.Add(cell);
        }
        DungeonGridGenerator.Instance.MoveMap(gameObject.transform.localScale.y);
        MapManager.Instance.ChangeMapState(false);
    }

    private void GenerateEncounter()
    {
        switch (MyEncounter)
        {
            case EEncounterType.NONE:
                break;
            case EEncounterType.FIGHTEASY:
                SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.FIGHTMEDIUM:
                SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.FIGHTHARD:
                SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
                break;
            case EEncounterType.BOSS:
                PlayerController.Instance.Die();
                break;
            case EEncounterType.HEAL:
                break;
            default:
                break;
        }
        DungeonGridGenerator.Instance.CurrentEncounter = MyEncounter;
        DungeonGridGenerator.Instance.CurrentEncounterIndex = MyEncounterIndex;

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
