using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncounterCell : MonoBehaviour
{
    public List<EncounterCell> NextCells = new List<EncounterCell>();
    public List<UILineRenderer> CellConnection = new List<UILineRenderer>();
    public bool IsConnected = false;
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
                    GetComponent<Image>().sprite = easyEncounter;
                    break;
                case EEncounterType.FIGHTMEDIUM:
                    GetComponent<Image>().sprite = mediumEncounter;
                    break;
                case EEncounterType.FIGHTHARD:
                    GetComponent<Image>().sprite = hardEncounter;
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
    //public Image myImage;
    [SerializeField]
    Sprite easyEncounter, mediumEncounter, hardEncounter;

    private void Start()
    {
    }

    public void StartEncounter()
    {
        if (Clickable == true)
        {
            foreach (EncounterCell cell in DungeonGridGenerator.Instance.SelectableCells)
            {
                //cell.gameObject.GetComponent<Image>().color = Color.black;
                cell.Clickable = false;
            }
            DungeonGridGenerator.Instance.SelectableCells.Clear();
            foreach (EncounterCell cell in NextCells)
            {
                cell.gameObject.GetComponent<Image>().color = Color.white;
                cell.Clickable = true;
                DungeonGridGenerator.Instance.SelectableCells.Add(cell);
            }
            DungeonGridGenerator.Instance.MoveMap(gameObject.transform.localScale.y);
            SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
            HideMap.Instance.ChangeMapState();
        }
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
