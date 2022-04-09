using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntElite : Enemy
{
    [Space]
    Vector3 playerPosition;
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
        enemyNavMesh = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (isAggro)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Attack();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (moveCounter >= 0)
        {
            enemyNavMesh.SetDestination(playerPosition);
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
            playerPosition = ((playerPosition - transform.position).normalized * JumpingDistance) + transform.position;
        }
    }

    private void Attack()
    {
        if (currentShotCooldown < 0)
        {
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            GameObject tmp = Instantiate(Projectile, transform.position, Quaternion.identity);
            tmp.GetComponent<GruntEliteProjectile>().MyDamage = damage;
            tmp.GetComponent<GruntEliteProjectile>().mySpeed = projectileSpeed;
            tmp.GetComponent<Rigidbody>().AddForce(((playerPosition - transform.position).normalized * projectileSpeed), ForceMode.Impulse);
            currentShotCooldown = timeForEachShot;
        }
        else
        {
            currentShotCooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(damage);
        }
    }
}
