using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    PlayerController player;
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
        player = PlayerController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == PlayerInventory.Instance.transform)
        {
            enemyController.isInRange = true;
            meshRenderer.material = aggroMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player.transform.GetChild(0))
        {
            enemyController.isInRange = false;
            meshRenderer.material = oldMaterial;
        }
    }
}
