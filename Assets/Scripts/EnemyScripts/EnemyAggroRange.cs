using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    [SerializeField]
    Enemy enemyController;
    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    Material aggroMaterial;
    Material oldMaterial;


    private void Start()
    {
        oldMaterial = meshRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyController.isAggro = true;
        meshRenderer.material = aggroMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        enemyController.isAggro = false;
        meshRenderer.material = oldMaterial;
    }
}
