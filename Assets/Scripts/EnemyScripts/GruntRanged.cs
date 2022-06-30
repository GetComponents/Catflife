using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntRanged : Enemy
{

    [Space]
    Vector3 playerPosition;
    //Vector3 navmeshDestination;
    public float maxMoveCounter, MaxStopCounter, JumpingDistance;
    //float moveCounter, stopCounter;

    [Space]
    [SerializeField]
    GameObject Projectile;
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    float timeForEachShot;
    float currentShotCooldown;

    private Animator myAnimator;

    private void Start()
    {
        playerPosition = PlayerController.Instance.transform.GetChild(0).position;
        //navmeshDestination = playerPosition;
        enemyNavMesh = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
    }
    new void Update()
    {
        if (isAggro)
        {
            Attack();
        }
        Vector3 tmp = (PlayerInventory.Instance.transform.position - transform.position).normalized;
        transform.forward = new Vector3(tmp.x, 0, tmp.z);
        base.Update();

    }

    private void Attack()
    {
        if (currentShotCooldown < 0)
        {
            currentShotCooldown = Random.Range(timeForEachShot * 0.5f, timeForEachShot * 2f);
            myAnimator.SetBool("attack", true);
        }
        else
        {
            currentShotCooldown -= Time.deltaTime * slowedSpeed;
        }
    }

    public override void TakeDamage(float amount)
    {
        //PlaySound GruntRangedDamaged
        base.TakeDamage(amount);
    }

    //triggered via animation, shoots a projectile
    private void Shoot()
    {
        AkSoundEngine.PostEvent("Play_GruntSpitAttack", this.gameObject);
        myAnimator.SetBool("attack", false);
        playerPosition = PlayerInventory.Instance.transform.position;
        GameObject tmp = Instantiate(Projectile, transform.position, Quaternion.identity);
        tmp.GetComponent<GruntProjectile>().MyDamage = damage;
        tmp.GetComponent<Rigidbody>().AddForce(((playerPosition - transform.position).normalized * projectileSpeed * slowedSpeed), ForceMode.Impulse);
        StartCoroutine(Knockback());
    }

    private IEnumerator Knockback()
    {
        enemyNavMesh.SetDestination((-(PlayerInventory.Instance.transform.position - transform.position).normalized * JumpingDistance) + transform.position);
        yield return new WaitForSeconds(maxMoveCounter);
        enemyNavMesh.SetDestination(transform.position);
    }
}
