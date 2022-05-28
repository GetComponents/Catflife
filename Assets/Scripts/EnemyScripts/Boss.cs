using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public float HealthPoints
    {
        get => m_healthPoints;
        set
        {
            if (value <= 0)
            {
                phase++;
                m_healthPoints = maxHealth;
            }
            else
            {
                m_healthPoints = value;
            }
        }
    }
    private float m_healthPoints;

    [SerializeField]
    private float maxHealth;

    List<int> currentAttack;

    [SerializeField]
    Vector2 timeToAttack, timeToCast, timeToCastRandom;

    [SerializeField]
    Vector2 randomCastDistance;

    [SerializeField]
    private float projectileSpeed, spinSpeed;

    Rigidbody rb;

    Vector3 spinVector;

    private int phase
    {
        get => m_phase;
        set
        {
            m_phase = value;
            switch (value)
            {
                case 1:
                    bossAnimator.SetBool("ChangePhase", true);
                    //PlaySound BossScream
                    break;
                case 2:
                    StartCoroutine(CastRandomly());
                    bossAnimator.SetBool("ChangePhase", true);
                    //Playound BossScream
                    break;
                case 3:
                    //PlaySound PlayerDeath
                    StopAllCoroutines();
                    bossAnimator.SetBool("Die", true);
                    break;
                default:
                    break;
            }
        }
    }
    private int m_phase = 0;

    [SerializeField]
    float selfcastDelay;

    [SerializeField]
    GameObject fireballPrefab, castPrefab, selfCastPrefab;

    [SerializeField]
    BoxCollider sword;

    [SerializeField]
    Transform handPos;

    [SerializeField]
    Animator bossAnimator;

    [SerializeField]
    NavMeshAgent bossNavmesh;

    [SerializeField]
    float movementSpeed;

    public bool PlayerIsInRange;

    public int SpawnedEnemiesAmount;

    private void Start()
    {
        currentAttack = new List<int>();
        rb = GetComponent<Rigidbody>();
        bossNavmesh.speed = movementSpeed;
        HealthPoints = maxHealth;
        StartCoroutine(Attack());
        StartCoroutine(TimeToCast());
    }

    private void Update()
    {
        bossNavmesh.destination = PlayerInventory.Instance.transform.position;
        if (spinVector != Vector3.zero)
        {
            rb.AddForce(spinVector / 10, ForceMode.VelocityChange);
        }
    }

    public void TakeDamage(float _damage)
    {
        HealthPoints -= _damage;
        //PlaySound BossHurt
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeToAttack.x, timeToAttack.y));
            if (phase >= 1 && Random.Range(0f, 1f) <= 0.33f)
            {
                //PlaySound BossCharging
                bossAnimator.SetInteger("AttackType", 4);
                currentAttack.Add(4);
            }
            else if (PlayerIsInRange)
            {
                bossAnimator.SetInteger("AttackType", 1);
                //PlaySound BossSwordSwing (Spieler SwordSwing runtergepitched)
                currentAttack.Add(1);
            }
            else
            {
                bossAnimator.SetInteger("AttackType", 2);
                currentAttack.Add(2);
            }
        }
    }

    IEnumerator TimeToCast()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeToCast.x, timeToCast.y));
            bossAnimator.SetInteger("AttackType", 3);
            currentAttack.Add(3);
        }
    }

    IEnumerator CastRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeToCastRandom.x, timeToCastRandom.y));
            Instantiate(castPrefab,
                    Vector3.Scale(new Vector3(1, 0, 1), PlayerInventory.Instance.transform.position) +
                    new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(randomCastDistance.x, randomCastDistance.y),
                Quaternion.identity);
        }
    }

    private void CastOnSelf()
    {
        Debug.Log("Casting on self");
        MageCast tmp = Instantiate(selfCastPrefab, transform.position, Quaternion.identity).GetComponent<MageCast>();
        tmp.TimeUntilExplosion = selfcastDelay;
        bossNavmesh.speed = 0;
    }

    private void AnimSpin()
    {
        bossAnimator.SetBool("Spinning", true);
        //PlaySound SpinMove (Loop)
        bossNavmesh.speed = 0;
        spinVector = (PlayerInventory.Instance.transform.position - transform.position).normalized * spinSpeed;
        rb.AddForce(spinVector, ForceMode.VelocityChange);
    }

    private void AnimCast()
    {
        Instantiate(castPrefab, Vector3.Scale(new Vector3(1, 0, 1), PlayerInventory.Instance.transform.position), Quaternion.identity);
    }

    private void AnimFireball()
    {
        GruntEliteProjectile tmp = Instantiate(fireballPrefab, handPos.position, Quaternion.identity).GetComponent<GruntEliteProjectile>();
        //PlaySound FireballCast (vom Spieler)
        tmp.mySpeed = projectileSpeed;
        tmp.MyDamage = 2;
        tmp.GetComponent<Rigidbody>().AddForce((PlayerInventory.Instance.transform.position - transform.position).normalized * projectileSpeed, ForceMode.Impulse);
    }

    private void AnimFinishAttack()
    {
        if (currentAttack[0] == 4)
        {
            bossNavmesh.speed = movementSpeed;
        }
        currentAttack.RemoveAt(0);
        if (currentAttack.Count == 0)
        {
            bossAnimator.SetInteger("AttackType", 0);
        }
    }

    private void EndPhaseChange()
    {
        bossAnimator.SetBool("ChangePhase", false);
        bossNavmesh.speed = movementSpeed;
    }

    void StartDeath()
    {
        bossAnimator.SetBool("Die", false);
    }
    void Die()
    {     
        SceneTransition.Instance.ChangeScene("EndScene", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            bossAnimator.SetBool("Spinning", false);
            spinVector = Vector3.zero;
            sword.enabled = false;
        }
    }
}
