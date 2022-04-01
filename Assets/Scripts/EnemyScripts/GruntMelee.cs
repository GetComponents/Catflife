using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace EnemyAI
{
    public class GruntMelee : Enemy
    {
        Vector3 playerPosition;
        NavMeshAgent thisEnemy;
        public float maxMoveCounter, MaxStopCounter, JumpingDistance;
        float moveCounter, stopCounter;

        void Start()
        {
            //stopCounter = Random.Range(0f, 3f);
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            thisEnemy = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (isAggro)
            {
                MoveTowardsPlayer();                
            }
        }

        private void MoveTowardsPlayer()
        {
            if (moveCounter >= 0)
            {
                thisEnemy.SetDestination(playerPosition);
                moveCounter -= Time.deltaTime;
            }
            else if (stopCounter >= 0)
            {
                thisEnemy.SetDestination(transform.position);
                stopCounter -= Time.deltaTime;
            }
            else
            {
                moveCounter = maxMoveCounter;
                stopCounter = MaxStopCounter;
                playerPosition = PlayerController.Instance.transform.GetChild(0).position;
                playerPosition = ((playerPosition - transform.position).normalized * JumpingDistance) + playerPosition;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController.Instance.HurtPlayer(damage);
            }
        }
    }
}
