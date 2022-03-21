using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGridGenerator : MonoBehaviour
{
    public static DungeonGridGenerator Instance;
    [SerializeField]
    private int GridWidth, GridHeight;

    [SerializeField]
    float nodeSpacing;

    [SerializeField]
    private float cellSpawnChance = 0.5f;
    public Cell[,] DungeonGrid;

    private List<Cell> unonnectedCells = new List<Cell>();

    [SerializeField]
    GameObject cellPrefab;

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
                    Cell currentCell = DungeonGrid[x, y];
                    unonnectedCells.Add(currentCell);
                    currentCell.ContainsEncounter = true;

                    currentCell.MyView = Instantiate(cellPrefab);
                    currentCell.MyView.transform.position = new Vector3(x, 0, y) * nodeSpacing;
                    cellSpawned = true;
                }
            }
            if (!cellSpawned)
            {
                y--;
            }
        }
    }

    private void ConnectCells()
    {
        ConnectBottomToTop();
        ConnectTopToBottom();
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
                    if (connectedCell.MyView != null)
                    {
                        currentCell.MyView.GetComponent<EncounterCell>().NextCells.Add(connectedCell.MyView.GetComponent<EncounterCell>());
                        unonnectedCells.Remove(connectedCell);
                        connectedCell.MyView.GetComponent<EncounterCell>().IsConnected = true;
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
}
