using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshBake : MonoBehaviour
{
    public static NavmeshBake Instance;

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

    public void Bake()
    {
        //We have to bake in runtime because the Dungeonarenas are spawned as Prefabs
        //Navmesh can not be baked for prefabs in editor
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
