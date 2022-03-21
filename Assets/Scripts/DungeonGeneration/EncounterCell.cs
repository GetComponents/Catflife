using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCell : MonoBehaviour
{
    public List<EncounterCell> NextCells = new List<EncounterCell>();
    public bool IsConnected = false;
    public EEncounterType MyEncounter;
    public bool Clickable;

    public void StartEncounter()
    {
        foreach (EncounterCell cell in NextCells)
        {
            cell.Clickable = true;
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
