using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace EnemyAI
{
    public class GruntMelee : Enemy
    {
        [Space]
        Vector3 playerPosition;
        public float maxMoveCounter, MaxStopCounter, JumpingDistance;
        float moveCounter, stopCounter;

        [SerializeField]
        Animator myAnimator;

        void Start()
        {
            playerPosition = PlayerController.Instance.transform.GetChild(0).position;
            enemyNavMesh = GetComponent<NavMeshAgent>();
        }

        new void Update()
        {
            base.Update();
            if (isAggro)
            {
                MoveTowardsPlayer();
            }
        }

        private void MoveTowardsPlayer()
        {

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
                myAnimator.SetBool("attack", true);
            }
        }

        public override void TakeDamage(float _amount)
        {
            //PlaySound GruntMeleeDamaged
            base.TakeDamage(_amount);
        } 

        private void EndAttackAnim()
        {
            myAnimator.SetBool("attack", false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player" && myAnimator.GetBool("attack"))
            {
                PlayerController.Instance.TakeDamage(damage);
            }
        }
    }
}
