using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bowler : Enemy
{
    PlayerController player;
    [SerializeField]
    float timeToAttack;
    [SerializeField]
    float timeStunned;

    public float timeUntilAttack;
    bool isAttacking;
    Vector3 attackDirection;

    private void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        player = PlayerController.Instance;
    }

    void Update()
    {
        if (timeUntilAttack < 0)
        {
            StartAttack();
        }
        if (!isAttacking)
        {
            timeUntilAttack -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        timeUntilAttack = timeToAttack;
        attackDirection = ((player.transform.GetChild(0).position - transform.position).normalized);
    }

    IEnumerator Stunned()
    {
        Debug.Log("IM AM SO STUNNED OMG");
        enemyNavMesh.SetDestination(transform.position);
        yield return new WaitForSeconds(timeStunned);
        isAttacking = false;
    }

    private void Attack()
    {
        enemyNavMesh.SetDestination(attackDirection + transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            StartCoroutine(Stunned());
        }
        else
        {
            Debug.Log(collision.transform.tag);
        }
    }
}
