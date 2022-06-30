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

    new void Update()
    {
        if (isAggro)
        {
            //Melee attack
            MoveTowardsPlayer();
        }
        else
        {
            //Ranged attack
            Attack();
        }
        base.Update();
    }

    private void MoveTowardsPlayer()
    {
        //Shouldve been a FSM, but I already had it finished when I noticed
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
            AkSoundEngine.PostEvent("Play_GruntJumpAttack", this.gameObject);
            moveCounter = maxMoveCounter;
            stopCounter = MaxStopCounter;
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            playerPosition = ((playerPosition - transform.position).normalized * JumpingDistance) + transform.position;
        }
    }

    public override void TakeDamage(float amount)
    {
        AkSoundEngine.PostEvent("Play_EliteGruntHurt", this.gameObject);
        base.TakeDamage(amount);
    }

    private void Attack()
    {
        Vector3 temp = (PlayerInventory.Instance.transform.position - transform.position).normalized;
        transform.forward = new Vector3(temp.x, 0, temp.z);
        if (currentShotCooldown < 0)
        {
            AkSoundEngine.PostEvent("Play_GruntSpitAttack", this.gameObject);
            //shoots a projectile
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            GameObject tmp = Instantiate(Projectile, transform.position, Quaternion.identity);
            tmp.GetComponent<GruntEliteProjectile>().MyDamage = damage;
            tmp.GetComponent<GruntEliteProjectile>().mySpeed = projectileSpeed * slowedSpeed;
            tmp.GetComponent<Rigidbody>().AddForce(((playerPosition - transform.position).normalized * projectileSpeed * slowedSpeed), ForceMode.Impulse);
            currentShotCooldown = timeForEachShot;
        }
        else
        {
            currentShotCooldown -= Time.deltaTime * slowedSpeed;
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
