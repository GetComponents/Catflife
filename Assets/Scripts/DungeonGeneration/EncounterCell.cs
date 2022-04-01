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
    public EEncounterType MyEncounter;
    public bool Clickable;

    private void Start()
    {
        
    }

    public void StartEncounter()
    {
        if (Clickable == true)
        {
            foreach (EncounterCell cell in DungeonGridGenerator.Instance.SelectableCells)
            {
                cell.gameObject.GetComponent<Image>().color = Color.black;
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
    FIGHT,
    BOSS,
    HEAL
}
