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

    bool hasSeenPlayer;

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
        timeUntilAttack = Random.Range(0f, 1f);
        enemyNavMesh = GetComponent<NavMeshAgent>();
        if (isElite)
            StartCoroutine(CastRandomly());
    }

    new void Update()
    {
        if (isAggro)
        {
            Move();
        }
        else if (seesPlayer || hasSeenPlayer)
        {
            hasSeenPlayer = true;
            if (timeUntilAttack < 0)
            {
                StartAttack();
            }
            else if (!isAttacking)
            {
                timeUntilAttack -= Time.deltaTime * slowedSpeed;
            }
        }
        base.Update();
    }

    private void StartAttack()
    {
        timeUntilAttack = timeToAttack;
        myAnimator.SetBool("isAttacking", true);
        isAttacking = true;
    }

    //Runs away from Player
    private void Move()
    {
        myAnimator.SetBool("isAttacking", false);
        myAnimator.SetBool("isFleeing", true);
        enemyNavMesh.SetDestination((-(PlayerController.Instance.transform.GetChild(0).position - transform.position).normalized
            * targetPointDistance) + transform.position);
        if (!isInRange)
        {
            isAggro = false;
            myAnimator.SetBool("isFleeing", false);
            isAttacking = false;
        }
    }

    public override void TakeDamage(float amount)
    {
        AkSoundEngine.PostEvent("Play_MageHurt", this.gameObject);
        base.TakeDamage(amount);
    }

    //Triggered by animat
    private void StartCast()
    {
        AkSoundEngine.PostEvent("Play_MageMumble", this.gameObject);
        MageCast tmp = Instantiate(AOEAttack, PlayerController.Instance.transform.GetChild(0).position + new Vector3(0, 0.2f, 0), Quaternion.identity).GetComponent<MageCast>();
        tmp.MyDamage = damage;
        tmp.TimeUntilExplosion = AOEExplosionTime;
        tmp.TimeUntilFaded = AOEFadeTime;
    }

    private void EndCast()
    {
        myAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    /// <summary>
    /// Casts in a radius around the Player
    /// </summary>
    /// <returns></returns>
    IEnumerator CastRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(EliteRandomCastTimer.x, EliteRandomCastTimer.y) / slowedSpeed);
            if (!isAggro && seesPlayer)
            {
                MageCast tmp = Instantiate(AOEAttack,
                    PlayerController.Instance.transform.GetChild(0).position +
                    new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(EliteCastDistance.x, EliteCastDistance.y)
                    + new Vector3(0, 0.2f, 0),
                    Quaternion.identity).GetComponent<MageCast>();
                tmp.MyDamage = damage;
                tmp.TimeUntilExplosion = AOEExplosionTime;
                tmp.TimeUntilFaded = AOEFadeTime;
            }
        }
    }
}
