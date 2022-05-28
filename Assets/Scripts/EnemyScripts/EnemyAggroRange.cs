using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    PlayerInventory player;
    [SerializeField]
    Enemy enemyController;


    private void Start()
    {
        player = PlayerInventory.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player == null)
        {
            player = PlayerInventory.Instance;
            return;
        }
        if (other.transform == player.transform)
        {
            enemyController.isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null)
        {
            player = PlayerInventory.Instance;
            return;
        }
        if (other.transform == player.transform)
        {
            enemyController.isInRange = false;
        }
    }
}
