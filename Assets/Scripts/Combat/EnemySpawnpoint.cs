using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField]
    GameObject Enemy;

    private void Start()
    {
        //There was a system before, where you could return to cleared arenas
        //This is kindo of deprecated now
        if (!NewDungeonGridGenerator.Instance.ClearedArena)
        {
            if (Enemy != null)
            {
                Instantiate(Enemy, transform.position, Quaternion.identity);
                CombatSceneChange.Instance.AddEnemy();
            }
        }
        else
        {
            CombatSceneChange.Instance.EndCombat();
        }
    }
}
