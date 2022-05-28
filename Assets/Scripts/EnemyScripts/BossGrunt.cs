using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossGrunt : Enemy
{

    [Space]
    Vector3 playerPosition;
    public float maxMoveCounter, MaxStopCounter, JumpingDistance;
    float moveCounter, stopCounter;

    Boss boss;

    void Start()
    {
        boss = FindObjectOfType<Boss>();
        boss.SpawnedEnemiesAmount++;
        playerPosition = PlayerController.Instance.transform.GetChild(0).position;
        enemyNavMesh = GetComponent<NavMeshAgent>();
    }

    new void Update()
    {
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
        base.Update();
    }

    private void MoveTowardsPlayer()
    {
        if (moveCounter >= 0)
        {
            enemyNavMesh.SetDestination(playerPosition);
            moveCounter -= Time.deltaTime * slowedSpeed;
        }
        else if (stopCounter >= 0)
        {
            enemyNavMesh.SetDestination(transform.position);
            stopCounter -= Time.deltaTime * slowedSpeed;
        }
        else
        {
            moveCounter = maxMoveCounter;
            stopCounter = MaxStopCounter;
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            playerPosition = ((playerPosition - transform.position).normalized * JumpingDistance) + transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        Instantiate(PickupHP,
            new Vector3(transform.position.x, PlayerInventory.Instance.transform.position.y, transform.position.z),
            Quaternion.identity);
        boss.SpawnedEnemiesAmount--;
    }
}

