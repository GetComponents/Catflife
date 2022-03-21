using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterCell : MonoBehaviour
{
    public List<EncounterCell> NextCells = new List<EncounterCell>();
    public bool IsConnected = false;
    public EEncounterType MyEncounter;
    public bool Clickable;

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
