using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMeshGenerator : MonoBehaviour
{
    public Mesh DungeonMesh;
    [SerializeField]
    private Material wallMaterial;
    public List<Vector3> meshVertices;
    public Vector2[] meshUVs;
    public List<int> meshIndices;
    private EncounterCell[] allEncounterCells;
    private Cell[,] allCells;
    public int gridWidth, gridHeight;
    int wallVertices;

    private void Start()
    {
        //DungeonMesh = GetComponent<MeshFilter>().mesh;
        DungeonMesh = gameObject.AddComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = wallMaterial;
        copyDungeonInformation();
        createVertices();
        createUVs();
        createIndices();
        DrawMesh();
        DungeonMesh.RecalculateNormals();
    }

    private void copyDungeonInformation()
    {
        allEncounterCells = FindObjectsOfType<EncounterCell>();
        allCells = DungeonGridGenerator.Instance.DungeonGrid;
        gridWidth = DungeonGridGenerator.Instance.GridWidth;
        gridHeight = DungeonGridGenerator.Instance.GridHeight;
    }

    private void createVertices()
    {
        meshVertices = new List<Vector3>();
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (allCells[i, j].MyView == null || allCells[i, j].MyView.transform == null)
                {
                    continue;
                }
                else
                {
                    for (int k = i; k < gridWidth; k++)
                    {
                        if (allCells[k, j].MyView != null && allCells[k, j].MyView.transform != null)
                        {
                            DrawWall(allCells[i, j], allCells[k, j]);
                        }
                    }
                }
            }
        }
    }

    private void DrawWall(Cell _leftCell, Cell _rightCell)
    {
        meshVertices.Add(_leftCell.MyView.GetComponent<EncounterCell>().RightWallPos - new Vector3(0, 3, 0));
        meshVertices.Add(_leftCell.MyView.GetComponent<EncounterCell>().RightWallPos + new Vector3(0, 3, 0));
        meshVertices.Add(_rightCell.MyView.GetComponent<EncounterCell>().LeftWallPos - new Vector3(0, 3, 0));
        meshVertices.Add(_rightCell.MyView.GetComponent<EncounterCell>().LeftWallPos + new Vector3(0, 3, 0));
        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 1);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 2);
        wallVertices += 4;
    }

    private void createUVs()
    {
        meshUVs = new Vector2[meshVertices.Count];
        for (int i = 0; i < meshUVs.Length; i += 4)
        {
            meshUVs[i + 0] = new Vector2(0, 0);
            meshUVs[i + 1] = new Vector2(1, 0);
            meshUVs[i + 2] = new Vector2(1, 0);
            meshUVs[i + 3] = new Vector2(1, 1);
        }
    }

    private void createIndices()
    {
        //meshIndices = new int[((int)(meshVertices.Count / 2) * 3) - 6];
        //for (int i = 0; i < meshIndices.Length; i += 6)
        //{

            
        //    //meshIndices[i + 0] = i + 0;
        //    //meshIndices[i + 1] = i + 1;
        //    //meshIndices[i + 2] = i + 3;
        //    //meshIndices[i + 3] = i + 0;
        //    //meshIndices[i + 5] = i + 3;
        //    //meshIndices[i + 4] = i + 2;
        //}
    }

    private void DrawMesh()
    {
        DungeonMesh.vertices = meshVertices.ToArray();
        DungeonMesh.uv = meshUVs;
        DungeonMesh.triangles = meshIndices.ToArray();
    }
}
