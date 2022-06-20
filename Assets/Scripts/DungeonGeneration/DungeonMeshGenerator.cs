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
    [HideInInspector]
    public int gridWidth, gridHeight;
    [SerializeField]
    private float wallThickness, wallHeight;
    int wallVertices;

    private void Start()
    {
        //Generates Dungeonmesh
        AddMeshComponents();
        copyDungeonInformation();
        createVerticesAndIndices();
        createUVs();
        DrawMesh();
        DungeonMesh.RecalculateNormals();
        gameObject.GetComponent<MeshCollider>().sharedMesh = DungeonMesh;
    }

    private void AddMeshComponents()
    {
        DungeonMesh = gameObject.AddComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = wallMaterial;
    }

    private void copyDungeonInformation()
    {
        allEncounterCells = FindObjectsOfType<EncounterCell>();
        allCells = NewDungeonGridGenerator.Instance.DungeonGrid;
        gridWidth = NewDungeonGridGenerator.Instance.GridWidth;
        gridHeight = NewDungeonGridGenerator.Instance.GridHeight;
    }

    private void createVerticesAndIndices()
    {
        meshVertices = new List<Vector3>();
        CreateHorizontalWalls();
        CreateVerticalWalls();
        CreateFloor();
    }

    private void CreateHorizontalWalls()
    {
        //Draw Walls Left to Right
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                //Leftmost wall
                if (i == 0)
                {
                    for (int k = 0; k < gridWidth; k++)
                    {
                        if (allCells[k, j].ContainsEncounter)
                        {
                            DrawHorizontalWall(allCells[i, j].MyView.transform.position
                                - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
                                + new Vector3(wallThickness, 0, wallThickness),
                                allCells[k, j].MyView.GetComponent<EncounterCell>().LeftWallPos.position);
                            break;
                        }
                    }
                }
                //Left Point
                if (allCells[i, j].ContainsEncounter)
                {
                    bool foundCell = false;
                    for (int k = i + 1; k < gridWidth; k++)
                    {
                        //Right Point
                        if (allCells[k, j].ContainsEncounter)
                        {
                            DrawHorizontalWall(allCells[i, j].MyView.GetComponent<EncounterCell>().RightWallPos.position,
                                allCells[k, j].MyView.GetComponent<EncounterCell>().LeftWallPos.position);
                            foundCell = true;
                            break;

                        }
                        //Rightmost Point
                        else if (k == gridWidth - 1)
                        {
                            DrawHorizontalWall(allCells[i, j].MyView.GetComponent<EncounterCell>().RightWallPos.position,
                                allCells[gridWidth - 1, j].MyView.transform.position
                                + new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x
                                + new Vector3(-wallThickness, 0, -wallThickness));
                            foundCell = true;
                            break;
                        }
                    }
                    //For the case that the rightmost point contains an encoutercell
                    if (!foundCell)
                    {
                        DrawHorizontalWall(allCells[i, j].MyView.GetComponent<EncounterCell>().RightWallPos.position,
                                allCells[gridWidth - 1, j].MyView.transform.position
                                + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
                                + new Vector3(-wallThickness, 0, -wallThickness));
                    }
                }
            }
        }
        //Draw the bottommost and topmost wall
        DrawHorizontalWall(allCells[0, 0].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
            + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
            + new Vector3(wallThickness, 0, wallThickness),
            allCells[gridWidth - 1, 0].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
             + new Vector3(-wallThickness, 0, -wallThickness));

        DrawHorizontalWall(allCells[0, gridHeight - 1].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
            - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
            + new Vector3(wallThickness, 0, wallThickness),
            allCells[gridWidth - 1, gridHeight - 1].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
             + new Vector3(-wallThickness, 0, -wallThickness));
    }

    private void CreateVerticalWalls()
    {
        //Left Wall
        DrawVerticalWall(allCells[0, 0].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
            + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
            + (new Vector3(1, 0, -1) * wallThickness),
            allCells[0, gridHeight - 1].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
            - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
            - (new Vector3(1, 0, -1) * wallThickness));
        //Right Wall
        DrawVerticalWall(allCells[gridWidth - 1, 0].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
             + (new Vector3(1, 0, -1) * wallThickness),
            allCells[gridWidth - 1, gridHeight - 1].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y)
             - (new Vector3(1, 0, -1) * wallThickness));
    }

    private void CreateFloor()
    {
        //Bottom left
        meshVertices.Add(allCells[0, 0].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y));
        //Top left
        meshVertices.Add(allCells[0, gridHeight - 1].MyView.transform.position
            - (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y));
        //Bottom right
        meshVertices.Add(allCells[gridWidth - 1, 0].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             + (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y));
        //Top right
        meshVertices.Add(allCells[gridWidth - 1, gridHeight - 1].MyView.transform.position
            + (new Vector3(1, 0, 1) * NewDungeonGridGenerator.Instance.nodeSpacing.x)
             - (new Vector3(1, 0, -1) * NewDungeonGridGenerator.Instance.nodeSpacing.y));

        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 1);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 2);
    }

    private void DrawHorizontalWall(Vector3 _leftPosition, Vector3 _topPosition)
    {
        //Frontface
        meshVertices.Add(_leftPosition + new Vector3(wallThickness, -wallHeight, -wallThickness));
        meshVertices.Add(_leftPosition + new Vector3(wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_topPosition + new Vector3(wallThickness, -wallHeight, -wallThickness));
        meshVertices.Add(_topPosition + new Vector3(wallThickness, wallHeight, -wallThickness));

        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 1);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 2);

        //Backface
        meshVertices.Add(_leftPosition + new Vector3(-wallThickness, -wallHeight, wallThickness));
        meshVertices.Add(_leftPosition + new Vector3(-wallThickness, wallHeight, wallThickness));
        meshVertices.Add(_topPosition + new Vector3(-wallThickness, -wallHeight, wallThickness));
        meshVertices.Add(_topPosition + new Vector3(-wallThickness, wallHeight, wallThickness));

        meshIndices.Add(wallVertices + 4);
        meshIndices.Add(wallVertices + 7);
        meshIndices.Add(wallVertices + 5);
        meshIndices.Add(wallVertices + 4);
        meshIndices.Add(wallVertices + 6);
        meshIndices.Add(wallVertices + 7);

        //TopFace
        meshVertices.Add(_leftPosition + new Vector3(wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_leftPosition + new Vector3(-wallThickness, wallHeight, wallThickness));
        meshVertices.Add(_topPosition + new Vector3(wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_topPosition + new Vector3(-wallThickness, wallHeight, wallThickness));

        meshIndices.Add(wallVertices + 8);
        meshIndices.Add(wallVertices + 9);
        meshIndices.Add(wallVertices + 11);
        meshIndices.Add(wallVertices + 8);
        meshIndices.Add(wallVertices + 11);
        meshIndices.Add(wallVertices + 10);

        wallVertices += 12;
    }

    private void DrawVerticalWall(Vector3 _bottomPosition, Vector3 _rightPosition)
    {
        //Leftface
        meshVertices.Add(_bottomPosition + new Vector3(wallThickness, -wallHeight, wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(wallThickness, wallHeight, wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(wallThickness, -wallHeight, wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(wallThickness, wallHeight, wallThickness));

        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 1);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 0);
        meshIndices.Add(wallVertices + 3);
        meshIndices.Add(wallVertices + 2);

        //Rightface
        meshVertices.Add(_bottomPosition + new Vector3(-wallThickness, -wallHeight, -wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(-wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(-wallThickness, -wallHeight, -wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(-wallThickness, wallHeight, -wallThickness));

        meshIndices.Add(wallVertices + 4);
        meshIndices.Add(wallVertices + 7);
        meshIndices.Add(wallVertices + 5);
        meshIndices.Add(wallVertices + 4);
        meshIndices.Add(wallVertices + 6);
        meshIndices.Add(wallVertices + 7);

        //TopFace
        meshVertices.Add(_bottomPosition + new Vector3(-wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(-wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(wallThickness, wallHeight, wallThickness));
        meshVertices.Add(_rightPosition + new Vector3(wallThickness, wallHeight, wallThickness));

        meshIndices.Add(wallVertices + 8);
        meshIndices.Add(wallVertices + 9);
        meshIndices.Add(wallVertices + 11);
        meshIndices.Add(wallVertices + 8);
        meshIndices.Add(wallVertices + 11);
        meshIndices.Add(wallVertices + 10);

        //Frontface
        meshVertices.Add(_bottomPosition + new Vector3(-wallThickness, -wallHeight, -wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(-wallThickness, wallHeight, -wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(wallThickness, -wallHeight, wallThickness));
        meshVertices.Add(_bottomPosition + new Vector3(wallThickness, wallHeight, wallThickness));

        meshIndices.Add(wallVertices + 12);
        meshIndices.Add(wallVertices + 13);
        meshIndices.Add(wallVertices + 15);
        meshIndices.Add(wallVertices + 12);
        meshIndices.Add(wallVertices + 15);
        meshIndices.Add(wallVertices + 14);

        wallVertices += 16;
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

    private void DrawMesh()
    {
        DungeonMesh.vertices = meshVertices.ToArray();
        DungeonMesh.uv = meshUVs;
        DungeonMesh.triangles = meshIndices.ToArray();
    }
}
