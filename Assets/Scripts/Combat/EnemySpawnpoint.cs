using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField]
    GameObject Enemy;

    private void Start()
    {
        if (!NewDungeonGridGenerator.Instance.ClearedArena)
        {
            Instantiate(Enemy, transform.position, Quaternion.identity);
            CombatSceneChange.Instance.AddEnemy();
        }
        else
        {
            CombatSceneChange.Instance.EndCombat();
        }
    }
}
