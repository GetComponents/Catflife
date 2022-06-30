using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    private uint playingID;

    private void Start()
    {
        playingID = AkSoundEngine.PostEvent("Play_WandererWhistle", this.gameObject);
        enemyNavMesh = GetComponent<NavMeshAgent>();
        myArm.GetComponent<WandererArm>().myDamage = damage;
    }

    new void Update()
    {
        if (isInRange && !isAttacking)
        {
            AkSoundEngine.PostEvent("Play_WandererAttack", this.gameObject);
            myAnimator.SetBool("isAttacking", true);
            isAttacking = true;
        }
        CreateNewDestination();
        if (isInRange)
        {
            Vector3 tmp = (PlayerInventory.Instance.transform.position - transform.position).normalized;
            transform.forward = new Vector3(tmp.x, 0, tmp.z);
        }
    }

    private void CreateNewDestination()
    {
        //Spawns a random Point where the Enemy walks to
        destinationChangeTimer -= Time.deltaTime * slowedSpeed;
        if (!isInRange && destinationChangeTimer < 0)
        {

            enemyNavMesh.SetDestination(transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * destinationDistance);
            destinationChangeTimer = timeToChangeDirection;
        }
    }

    public override void TakeDamage(float amount)
    {
        AkSoundEngine.PostEvent("Play_WandererHurt", this.gameObject);
        base.TakeDamage(amount);
    }

    private void StartAttack()
    {
        //myArm.SetActive(true);
    }

    private void EndAttack()
    {
        myAnimator.SetBool("isAttacking", false);
        //myArm.SetActive(false);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown / slowedSpeed);
        isAttacking = false;
    }

    private void OnDestroy()
    {
        AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
    }
}
