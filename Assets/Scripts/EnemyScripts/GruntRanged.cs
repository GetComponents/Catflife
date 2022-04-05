using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntRanged : Enemy
{

    [Space]
    Vector3 playerPosition;
    Vector3 navmeshDestination;
    public float maxMoveCounter, MaxStopCounter, JumpingDistance;
    float moveCounter, stopCounter;

    [Space]
    [SerializeField]
    GameObject Projectile;
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    float timeForEachShot;
    float currentShotCooldown;

    private void Start()
    {
        playerPosition = PlayerController.Instance.transform.GetChild(0).position;
        navmeshDestination = playerPosition;
        enemyNavMesh = GetComponent<NavMeshAgent>();
        isAggro = true;
    }
    void Update()
    {
        if (isAggro)
        {
            Move();
            Attack();
        }
    }

    private void Move()
    {
        if (moveCounter >= 0)
        {
            enemyNavMesh.SetDestination(navmeshDestination);
            moveCounter -= Time.deltaTime;
        }
        else if (stopCounter >= 0)
        {
            enemyNavMesh.SetDestination(transform.position);
            stopCounter -= Time.deltaTime;
        }
        else
        {
            moveCounter = maxMoveCounter;
            stopCounter = MaxStopCounter;
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            navmeshDestination = ((-(playerPosition - transform.position).normalized * JumpingDistance) + transform.position);
        }
    }

    private void Attack()
    {
        if (currentShotCooldown < 0)
        {
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            GameObject tmp = Instantiate(Projectile, transform.position, Quaternion.identity);
            tmp.GetComponent<GruntProjectile>().MyDamage = damage;
            tmp.GetComponent<Rigidbody>().AddForce(((playerPosition - transform.position).normalized * projectileSpeed), ForceMode.Impulse);
            currentShotCooldown = timeForEachShot;
        }
        else
        {
            currentShotCooldown -= Time.deltaTime;
        }
    }
}
