using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewDungeonGridGenerator : MonoBehaviour
{
    public static NewDungeonGridGenerator Instance;
    public int GridWidth, GridHeight;

    public Vector2 nodeSpacing;

    [SerializeField]
    private float cellSpawnChance = 0.5f;

    [SerializeField]
    private Transform map;
    public Cell[,] DungeonGrid;
    private Cell finalCell;

    [SerializeField]
    GameObject encounterCell, emptyCell;

    [SerializeField]
    Vector3 SectionOneEncounters, SectionTwoEncounters, SectionThreeEncounters;

    public List<GameObject> EasyEncounters, MediumEncounters, HardEncounters;

    public EEncounterType CurrentEncounter;
    public int CurrentEncounterIndex;
    public bool ClearedArena;

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
        GenerateGrid();
        RotateMap();
    }

    private void Start()
    {
        MapManager.Instance.ChangeMapState(true);
    }

    private void RotateMap()
    {
        map.transform.eulerAngles = new Vector3(0, -45, 0);
    }

    private void GenerateGrid()
    {
        DungeonGrid = new Cell[GridWidth, GridHeight];
        for (int y = 0; y < GridHeight; y++)
        {
            bool cellSpawned = false;
            for (int x = 0; x < GridWidth; x++)
            {
                DungeonGrid[x, y] = new Cell();
                if (Random.Range(0f, 1f) <= cellSpawnChance)
                {
                    Cell currentCell = DungeonGrid[x, y];
                    currentCell.MyView = Instantiate(encounterCell, map);
                    DungeonGrid[x, y].MyView.transform.position = new Vector3(x * nodeSpacing.x, 0, y * nodeSpacing.y);
                    DungeonGrid[x, y].MyView.name = $"{x + 1} / {y + 1} (FILLED)";
                    currentCell.ContainsEncounter = true;
                    cellSpawned = true;
                    if ((float)y / (float)GridHeight <= 1f / 3f)
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 1);
                    }
                    else if ((float)y / (float)GridHeight <= 2f / 3f)
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 2);
                    }
                    else
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 3);
                    }
                }
                else
                {
                    DungeonGrid[x, y].MyView = Instantiate(emptyCell, map);
                    DungeonGrid[x, y].MyView.transform.position = new Vector3(x * nodeSpacing.x, 0, y * nodeSpacing.y);
                    DungeonGrid[x, y].MyView.name = $"{x + 1} / {y + 1} (EMPTY)";
                    DungeonGrid[x, y].ContainsEncounter = false;
                }
            }
            if (!cellSpawned)
            {
                y--;
                //for (int x = 0; x < GridWidth; x++)
                //{
                //    Destroy(DungeonGrid[x, y].MyView);
                //}
            }
        }
        GenerateEnd();
    }

    private void SpawnEncounterCell()
    {
    }

    private void GenerateEncounterType(EncounterCell _cell, int _modifier)
    {
        float tmp = Random.Range(0f, 1f);
        switch (_modifier)
        {
            case 1:
                if (tmp <= SectionOneEncounters.x)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTEASY;
                }
                else if (tmp <= SectionOneEncounters.y)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTMEDIUM;
                }
                else if (tmp <= SectionOneEncounters.z)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTHARD;
                }
                break;
            case 2:
                if (tmp <= SectionTwoEncounters.x)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTEASY;
                }
                else if (tmp <= SectionTwoEncounters.y)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTMEDIUM;
                }
                else if (tmp <= SectionTwoEncounters.z)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTHARD;
                }
                break;
            case 3:
                if (tmp <= SectionThreeEncounters.x)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTEASY;
                }
                else if (tmp <= SectionThreeEncounters.y)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTMEDIUM;
                }
                else if (tmp <= SectionThreeEncounters.z)
                {
                    _cell.MyEncounter = EEncounterType.FIGHTHARD;
                }
                break;
            default:
                break;
        }
        switch (_cell.MyEncounter)
        {
            case EEncounterType.FIGHTEASY:
                _cell.MyEncounterIndex = Random.Range(0, EasyEncounters.Count);
                break;
            case EEncounterType.FIGHTMEDIUM:
                _cell.MyEncounterIndex = Random.Range(0, MediumEncounters.Count);
                break;
            case EEncounterType.FIGHTHARD:
                _cell.MyEncounterIndex = Random.Range(0, HardEncounters.Count);
                break;
            default:
                break;
        }
    }

    private void GenerateEnd()
    {
        finalCell = new Cell();
        finalCell.MyView = Instantiate(encounterCell, map);
        finalCell.MyView.transform.position = new Vector3(encounterCell.transform.localScale.x * nodeSpacing.x,
            0, GridHeight * encounterCell.transform.localScale.y * nodeSpacing.y);
        finalCell.MyView.GetComponent<EncounterCell>().MyEncounter = EEncounterType.BOSS;
    }
}
