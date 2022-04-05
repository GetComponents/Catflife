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
    [SerializeField]
    float rollSpeed;

    Rigidbody rb;

    public float timeUntilAttack;
    bool isAttacking;

    private void Start()
    {
        player = PlayerController.Instance;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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

    private void StartAttack()
    {
        isAttacking = true;
        timeUntilAttack = timeToAttack;
        rb.AddForce((player.transform.GetChild(0).position - transform.position).normalized * rollSpeed, ForceMode.Impulse);
    }

    IEnumerator Stunned()
    {
        Debug.Log("IM AM SO STUNNED OMG");
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(timeStunned);
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Player" || collision.transform.tag == "Enemy")
        {
            StartCoroutine(Stunned());
        }
    }
}
