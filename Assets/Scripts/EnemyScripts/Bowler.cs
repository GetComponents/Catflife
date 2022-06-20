using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Unused Enemy that rolls towards the Player
/// </summary>
public class Bowler : Enemy
{
    PlayerController player;
    [SerializeField]
    float timeToAttack;
    [SerializeField]
    float timeStunned;
    [SerializeField]
    float rollSpeed;

    Animator myAnimator;

    Rigidbody rb;

    public float timeUntilAttack;
    bool isAttacking;
    //Vector3 lastPlayerPos;

    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    int projectileDamage;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        player = PlayerController.Instance;
        rb = GetComponent<Rigidbody>();
    }

    new void Update()
    {
        base.Update();
        //if (seesPlayer)
        //{
        //    lastPlayerPos = player.transform.position;
        //}
        if (timeUntilAttack < 0)
        {
            StartAttack();
        }
        else if (!isAttacking)
        {
            timeUntilAttack -= Time.deltaTime * slowedSpeed;
        }
    }

    private void StartAttack()
    {
        myAnimator.SetBool("isAttacking", true);
        isAttacking = true;
        timeUntilAttack = timeToAttack;
        if (seesPlayer)
        {
            rb.AddForce((player.transform.GetChild(0).position - transform.position).normalized * rollSpeed * slowedSpeed, ForceMode.Impulse);
        }
        else
        {
            //RaycastHit hit;
            //float degreeChange;
            //do
            //{
            //    Physics.Raycast(transform.position, (PlayerController.Instance.transform.position - transform.position), out hit);

            //}

            //if ()
            //while ((hit.transform.position - transform.position).magnitude < (lastPlayerPos - transform.position).magnitude);
            rb.AddForce((player.transform.GetChild(0).position - transform.position).normalized * rollSpeed * slowedSpeed, ForceMode.Impulse);
        }
    }

    private void StunnedStart()
    {
        myAnimator.SetBool("isAttacking", false);
        rb.velocity = Vector3.zero;
    }

    private void StunnedEnd()
    {
        myAnimator.SetBool("isStunned", false);
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Enemy")
        {
            myAnimator.SetBool("isStunned", true);
            if (isElite)
            {
                ShootProjectiles();
            }
        }
        else if (collision.transform.tag == "Player")
        {
            myAnimator.SetBool("isStunned", true);
            PlayerController.Instance.TakeDamage(damage);
        }
    }

    private void ShootProjectiles()
    {
        Vector3 playerDirection = new Vector3(player.transform.GetChild(0).position.x - transform.position.x, 0, player.transform.GetChild(0).position.z - transform.position.z).normalized;
        for (int i = -45; i <= 45; i += 30)
        {
            GameObject tmp = Instantiate(projectile,
                new Vector3(transform.position.x, PlayerInventory.Instance.transform.position.y, transform.position.z),
                Quaternion.identity);
            tmp.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(0, i, 0) * playerDirection * projectileSpeed * slowedSpeed, ForceMode.Impulse);
            tmp.GetComponent<BowlerProjectile>().myDamage = projectileDamage;
        }
    }
}
