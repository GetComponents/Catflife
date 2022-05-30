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
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
