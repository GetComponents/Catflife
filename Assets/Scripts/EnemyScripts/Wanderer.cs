using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wanderer : Enemy
{
    [SerializeField]
    float timeToChangeDirection = 1;
    float destinationChangeTimer;
    [SerializeField]
    float destinationDistance = 1;
    [SerializeField]
    float attackCooldown;
    bool isAttacking;

    [SerializeField]
    GameObject myArm;

    [SerializeField]
    Animator myAnimator;

    private void Start()
    {
        //PlaySound whistle (loop)
        enemyNavMesh = GetComponent<NavMeshAgent>();
        myArm.GetComponent<WandererArm>().myDamage = damage;
    }

    new void Update()
    {
        if (isInRange && !isAttacking)
        {
            myAnimator.SetBool("isAttacking", true);
            isAttacking = true;
        }
        destinationChangeTimer -= Time.deltaTime * slowedSpeed;
        CreateNewDestination();
        Vector3 tmp = (PlayerController.Instance.transform.GetChild(0).position - transform.position).normalized;
        transform.forward = new Vector3(tmp.x, 0, tmp.z);
    }

    private void CreateNewDestination()
    {
        if (!isInRange && destinationChangeTimer < 0)
        {
        enemyNavMesh.SetDestination(transform.position + new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f)).normalized * destinationDistance);
            destinationChangeTimer = timeToChangeDirection;
        }
    }

    private void StartAttack()
    {
        //PlaySound WandererSwing/Scream
        myArm.SetActive(true);
    }

    private void EndAttack()
    {
        myAnimator.SetBool("isAttacking", false);
        myArm.SetActive(false);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown / slowedSpeed);
        isAttacking = false;
    }
}
