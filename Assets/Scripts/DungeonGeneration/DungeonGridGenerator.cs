using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Cell FinalCell;

    private List<Cell> unonnectedCells = new List<Cell>();
    public List<EncounterCell> SelectableCells = new List<EncounterCell>();

    [SerializeField]
    GameObject cellPrefab, cellLinePrefab, debugPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateGrid();
        ConnectCells();
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
                    currentCell.MyView.transform.position = (new Vector3((x - (int)(GridWidth * 0.5f)) * cellPrefab.transform.localScale.x, y * cellPrefab.transform.localScale.y, 0)
                        * nodeSpacing) + new Vector3(Screen.width * 0.5f, Screen.height * 0.1f);
                    cellSpawned = true;
                }
            }
            if (!cellSpawned)
            {
                y--;
            }
        }
        GenerateEnd();
    }

    private void GenerateEnd()
    {
        FinalCell = new Cell();
        FinalCell.MyView = Instantiate(cellPrefab, map);
        FinalCell.MyView.transform.position = (new Vector3(cellPrefab.transform.localScale.x, (GridHeight + 1) * cellPrefab.transform.localScale.y, 0)
                        * nodeSpacing) + new Vector3(Screen.width * 0.5f, Screen.height * 0.1f);
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
                        DungeonGrid[x, y].MyView.GetComponent<Image>().color = Color.white;
                        DungeonGrid[x, y].MyView.GetComponent<EncounterCell>().Clickable = true;
                        SelectableCells.Add(DungeonGrid[x, y].MyView.GetComponent<EncounterCell>());
                    }
                    else
                    {
                        DungeonGrid[x, y].MyView.GetComponent<Image>().color = Color.black;
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
                        currentCell.MyView.GetComponent<EncounterCell>().NextCells.Add(FinalCell.MyView.GetComponent<EncounterCell>());
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
            //if (xPos + x < GridWidth && xPos - x >= 0)
            //{

            //    Debug.Log($"{xPos + x} / {yPos + 1} is in Bounds of {GridWidth} / {GridHeight}");
            //}
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
        map.position = new Vector3(map.position.x, map.position.y - (amount * nodeSpacing));
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
