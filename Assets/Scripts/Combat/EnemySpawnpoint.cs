using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField]
    GameObject Enemy;

    private void Start()
    {
        Instantiate(Enemy, transform.position, Quaternion.identity);
    }
}
