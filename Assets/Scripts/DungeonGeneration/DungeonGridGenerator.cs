using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DungeonGridGenerator : MonoBehaviour
{
    public static DungeonGridGenerator Instance;
    [SerializeField]
    private int GridWidth, GridHeight;

    [SerializeField]
    float nodeSpacing;

    [SerializeField]
    private float cellSpawnChance = 0.5f;

    [SerializeField]
    private Transform map;
    public Cell[,] DungeonGrid;
    private Cell finalCell;

    private List<Cell> unonnectedCells = new List<Cell>();
    public List<EncounterCell> SelectableCells = new List<EncounterCell>();

    [SerializeField]
    GameObject cellPrefab, cellLinePrefab, debugPoint;

    [SerializeField]
    Vector3 SectionOneEncounters, SectionTwoEncounters, SectionThreeEncounters;

    public List<GameObject> EasyEncounters, MediumEncounters, HardEncounters;

    public EEncounterType CurrentEncounter;
    public int CurrentEncounterIndex;

    [SerializeField]
    public Light myLight;
    private Camera gameCamera;
    private InputAction click;

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
        gameCamera = FindObjectOfType<Camera>();
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += ctx => {
            RaycastHit hit;
            Vector3 coor = Mouse.current.position.ReadValue();
            if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
            {
                EncounterCell tmp = hit.transform.GetComponent<EncounterCell>();
                if (tmp != null)
                {
                    tmp.StartEncounter();
                }
            }
        };
        click.Enable();
    }

    private void Start()
    {
        MapManager.Instance.ChangeMapState(true);
        GenerateGrid();
        ConnectCells();
        RotateMap();
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
                    if (x == 0 && y == 0)
                        continue;
                    Cell currentCell = DungeonGrid[x, y];
                    unonnectedCells.Add(currentCell);
                    currentCell.ContainsEncounter = true;

                    currentCell.MyView = Instantiate(cellPrefab, map);
                    //currentCell.MyView.transform.position = (new Vector3((x - (int)(GridWidth * 0.5f)) * cellPrefab.transform.localScale.x, y * cellPrefab.transform.localScale.y, 0)
                    //    * nodeSpacing) + new Vector3(Screen.width * 0.5f, Screen.height * 0.1f);
                    currentCell.MyView.transform.position = (new Vector3((x - (int)(GridWidth * 0.5f)) * cellPrefab.transform.localScale.x, 0, y * cellPrefab.transform.localScale.y)
                        * nodeSpacing);
                    cellSpawned = true;

                    if ((float)y/(float)GridHeight <= 1f/3f)
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 1);
                    }
                    else if((float)y/(float)GridHeight <= 2f/3f)
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 2);
                    }
                    else
                    {
                        GenerateEncounterType(currentCell.MyView.GetComponent<EncounterCell>(), 3);
                    }
                }
            }
            if (!cellSpawned)
            {
                y--;
            }
        }
        GenerateEnd();
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
                _cell.MyEncounterIndex = Random.Range(0, EasyEncounters.Count - 1);
                break;
            case EEncounterType.FIGHTMEDIUM:
                _cell.MyEncounterIndex = Random.Range(0, MediumEncounters.Count - 1);
                break;
            case EEncounterType.FIGHTHARD:
                _cell.MyEncounterIndex = Random.Range(0, HardEncounters.Count - 1);
                break;
            default:
                break;
        }
    }

    private void GenerateEnd()
    {
        finalCell = new Cell();
        finalCell.MyView = Instantiate(cellPrefab, map);
        //finalCell.MyView.transform.position = (new Vector3(cellPrefab.transform.localScale.x, (GridHeight + 1) * cellPrefab.transform.localScale.y, 0)
        //                * nodeSpacing) + new Vector3(Screen.width * 0.5f, Screen.height * 0.1f);
        finalCell.MyView.transform.position = (new Vector3(cellPrefab.transform.localScale.x,0, (GridHeight + 1) * cellPrefab.transform.localScale.y)
                        * nodeSpacing);
        SelectableCells.Add(finalCell.MyView.GetComponent<EncounterCell>());
        finalCell.MyView.GetComponent<EncounterCell>().MyEncounter = EEncounterType.BOSS;
    }

    private void GenerateSeed()
    {
        //
    }

    private void ConnectCells()
    {
        ConnectBottomToTop();
        ConnectTopToBottom();
        UIColourChange();
    }

    private void UIColourChange()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                if (DungeonGrid[x, y].MyView != null)
                    if (y == 0)
                    {
                        //DungeonGrid[x, y].MyView.GetComponent<Image>().color = Color.white;
                        DungeonGrid[x, y].MyView.GetComponent<EncounterCell>().Clickable = true;
                        SelectableCells.Add(DungeonGrid[x, y].MyView.GetComponent<EncounterCell>());
                    }
                    else
                    {
                        //DungeonGrid[x, y].MyView.GetComponent<Image>().color = Color.red;
                        DungeonGrid[x, y].MyView.GetComponent<EncounterCell>().Clickable = false;
                    }
            }
        }
    }

    private void ConnectBottomToTop()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Cell currentCell = DungeonGrid[x, y];
                if (currentCell.ContainsEncounter)
                {
                    Cell connectedCell = FindClosestTopCell(x, y);

                    if (connectedCell.MyView != null && currentCell.MyView != null)
                    {

                        currentCell.MyView.GetComponent<EncounterCell>().NextCells.Add(connectedCell.MyView.GetComponent<EncounterCell>());
                        unonnectedCells.Remove(connectedCell);
                        connectedCell.MyView.GetComponent<EncounterCell>().IsConnected = true;

                        //Instantiate(cellLinePrefab, currentCell.MyView.transform);
                        //Transform tmp1 = currentCell.MyView.transform;
                        //Transform tmp2 = connectedCell.MyView.transform;
                        //cellLinePrefab.GetComponent<UILineRenderer>().points = new List<Vector2>
                        //{
                        //    new Vector2(0,0),
                        //    new Vector2((tmp2.position.x / tmp2.localScale.x) - (tmp1.position.x / tmp1.localScale.x),
                        //    (tmp2.position.y / tmp2.localScale.y) - (tmp1.position.y / tmp1.localScale.y)) * 0.1f
                        //    //(connectedCell.MyView.transform.position / cellPrefab.transform.localScale) - currentCell.MyView.transform.position
                        //};
                        //Debug.Log(cellLinePrefab.GetComponent<UILineRenderer>().points[1]);
                    }
                    else if (y == GridHeight - 1 && currentCell.MyView != null)
                    {
                        currentCell.MyView.GetComponent<EncounterCell>().NextCells.Add(finalCell.MyView.GetComponent<EncounterCell>());
                    }

                }
            }
        }
    }

    private void ConnectTopToBottom()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Cell currentCell = DungeonGrid[x, y];
                if (currentCell.ContainsEncounter && !currentCell.MyView.GetComponent<EncounterCell>().IsConnected)
                {
                    Cell connectedCell = FindClosestBottomCell(x, y);
                    if (connectedCell.MyView != null)
                        connectedCell.MyView.GetComponent<EncounterCell>().NextCells.Add(currentCell.MyView.GetComponent<EncounterCell>());
                    unonnectedCells.Remove(currentCell);
                    currentCell.MyView.GetComponent<EncounterCell>().IsConnected = true;

                    //Instantiate(cellLinePrefab, currentCell.MyView.transform);
                    //Transform tmp2 = currentCell.MyView.transform;
                    //Transform tmp1 = null;
                    //if (connectedCell.MyView != null)
                    //{
                    //    tmp1 = connectedCell.MyView.transform;
                    //}
                    //    cellLinePrefab.GetComponent<UILineRenderer>().points = new List<Vector2>
                    //    {
                    //        new Vector2(0,0),
                    //        new Vector2((tmp2.position.x / tmp2.localScale.x) - (tmp1.position.x / tmp1.localScale.x),
                    //        (tmp2.position.y / tmp2.localScale.y)- tmp1.position.y / tmp1.localScale.y) * 0.1f,
                    //        //(connectedCell.MyView.transform.position / cellPrefab.transform.localScale) - currentCell.MyView.transform.position
                    //    };
                }
            }
        }
    }

    private Cell FindClosestTopCell(int xPos, int yPos)
    {
        if (!(yPos + 1 < GridHeight))
        {
            //NOT THE END SOLUTION §
            return DungeonGrid[0, 0];
        }

        for (int x = 0; x < GridWidth; x++)
        {
            if (xPos + x < GridWidth && DungeonGrid[xPos + x, yPos + 1].ContainsEncounter)
            {
                return DungeonGrid[xPos + x, yPos + 1];
            }

            if (xPos - x >= 0 && DungeonGrid[xPos - x, yPos + 1].ContainsEncounter)
            {
                return DungeonGrid[xPos - x, yPos + 1];
            }
        }
        Debug.Log($"Cell[{xPos},{yPos}]  Cell is null");
        return null;
    }

    private Cell FindClosestBottomCell(int xPos, int yPos)
    {
        if (yPos == 0)
        {
            //NOT THE END SOLUTION §
            return DungeonGrid[0, 0];
        }

        for (int x = 0; x < GridWidth; x++)
        {
            if (xPos + x < GridWidth && DungeonGrid[xPos + x, yPos - 1].ContainsEncounter)
            {
                return DungeonGrid[xPos + x, yPos - 1];
            }

            if (xPos - x >= 0 && DungeonGrid[xPos - x, yPos - 1].ContainsEncounter)
            {
                return DungeonGrid[xPos - x, yPos - 1];
            }
        }
        Debug.Log($"Cell[{xPos},{yPos}] Bottom Cell is null");
        return null;
    }

    public void MoveMap(float amount)
    {
        //map.position = new Vector3(map.position.x, map.position.y - (amount * nodeSpacing));
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            foreach (Cell cell in DungeonGrid)
            {
                if (cell.MyView != null)
                {
                    foreach (EncounterCell nextCell in cell.MyView.GetComponent<EncounterCell>().NextCells)
                    {
                        Gizmos.DrawLine(cell.MyView.transform.position, nextCell.gameObject.transform.position);
                    }
                }
            }
    }
}
