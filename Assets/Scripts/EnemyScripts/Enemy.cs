using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/// <summary>
/// Script that all normal enemies inherit from
/// </summary>
public class Enemy : MonoBehaviour
{
    public float HealthPoints
    {
        get => m_healthPoints;
        set
        {
            if (value <= 0)
            {
                m_healthPoints = 0;
                OnDeath.Invoke();
                Destroy(gameObject);
            }
            m_healthPoints = value;
        }
    }
    [SerializeField]
    private float m_healthPoints;
    public bool isAggro, isInRange;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected bool isElite, needsToSeePlayer;
    protected bool seesPlayer;
    protected NavMeshAgent enemyNavMesh;
    [SerializeField]
    protected GameObject PickupHP, PickupEnergy;
    public int BaseEnergyPickupAmount = 10;
    [HideInInspector]
    public float originalNavMeshSpeed, slowedSpeed = 1;

    [SerializeField]
    private GameObject deathVFX;

    public UnityEvent OnDeath;

    [SerializeField]
    public LayerMask raycastLayerMask;

    private void Awake()
    {
        OnDeath.AddListener(Die);
        HealthPoints = m_healthPoints;
        if (GetComponent<NavMeshAgent>())
            originalNavMeshSpeed = GetComponent<NavMeshAgent>().speed;
    }

    public void Update()
    {
        //System where the enemy tries to see the Player
        //In a game with a bigger scale I wouldn't fo it this way
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (PlayerInventory.Instance.transform.position - transform.position).normalized,
            out hit, Mathf.Infinity, raycastLayerMask) && (hit.transform.gameObject.tag == "Player"))
        {
            seesPlayer = true;
            if (isInRange)
            {
                isAggro = true;
            }
        }
        else
        {
            seesPlayer = false;
        }
    }

    public void TakeDamage(float amount)
    {
        HealthPoints -= amount;
    }

    protected virtual void Die()
    {
        PickupEnergy energyPickup = Instantiate(PickupEnergy,
            new Vector3(transform.position.x, PlayerInventory.Instance.transform.position.y, transform.position.z),
            Quaternion.identity).GetComponent<PickupEnergy>();
        energyPickup.EnergyGainAmount = BaseEnergyPickupAmount;
        //Spawns HP-pickup
        Instantiate(PickupHP,
            new Vector3(transform.position.x, PlayerInventory.Instance.transform.position.y, transform.position.z),
            Quaternion.identity);
        Destroy(Instantiate(deathVFX, transform.position, Quaternion.identity), 1);
        CombatSceneChange.Instance?.RemoveEnemy();
    }
}
