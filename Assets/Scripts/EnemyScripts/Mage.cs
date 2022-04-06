using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mage : Enemy
{
    [SerializeField]
    GameObject AOEAttack;
    [SerializeField]
    float AOEExplosionTime, AOEFadeTime;


    [SerializeField]
    float timeToAttack;
    float timeUntilAttack;

    float targetPointDistance = 1;
    bool isAttacking;

    [SerializeField]
    Animator myAnimator;

    [Space]
    [SerializeField]
    Vector2 EliteRandomCastTimer, EliteCastDistance;

    private void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        if (isElite)
            StartCoroutine(CastRandomly());
    }

    private void Update()
    {
        if (isAggro)
        {
            Move();
        }
        else
        {
            if (timeUntilAttack < 0)
            {
                StartAttack();
            }
            else if (!isAttacking)
            {
                timeUntilAttack -= Time.deltaTime;
            }
        }

    }

    private void StartAttack()
    {
        myAnimator.SetBool("isAttacking", true);
        isAttacking = true;
    }

    private void Move()
    {
        enemyNavMesh.SetDestination((-(PlayerController.Instance.transform.GetChild(0).position - transform.position).normalized
            * targetPointDistance) + transform.position);
    }

    private void StartCast()
    {
        MageCast tmp = Instantiate(AOEAttack, PlayerController.Instance.transform.GetChild(0).position, Quaternion.identity).GetComponent<MageCast>();
        tmp.MyDamage = damage;
        tmp.TimeUntilExplosion = AOEExplosionTime;
        tmp.TimeUntilFaded = AOEFadeTime;
    }

    private void EndCast()
    {
        if (isElite)
        {
            MageCast tmp = Instantiate(AOEAttack, PlayerController.Instance.transform.GetChild(0).position, Quaternion.identity).GetComponent<MageCast>();
            tmp.MyDamage = damage;
            tmp.TimeUntilExplosion = AOEExplosionTime;
            tmp.TimeUntilFaded = AOEFadeTime;
        }
        myAnimator.SetBool("isAttacking", false);
    }

    IEnumerator CastRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(EliteRandomCastTimer.x, EliteRandomCastTimer.y));
            if (!isAggro)
            {
                MageCast tmp = Instantiate(AOEAttack,
                    PlayerController.Instance.transform.GetChild(0).position +
                    new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(EliteCastDistance.x, EliteCastDistance.y),
                    Quaternion.identity).GetComponent<MageCast>();
                tmp.MyDamage = damage;
                tmp.TimeUntilExplosion = AOEExplosionTime;
                tmp.TimeUntilFaded = AOEFadeTime;
            }
        }
    }
}
