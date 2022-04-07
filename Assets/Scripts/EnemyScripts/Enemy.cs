using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float HealthPoints
    {
        get => m_healthPoints;
        set
        {
            if (value <= 0)
            {
                Die();
            }
            m_healthPoints = value;
        }
    }
    [SerializeField]
    private float m_healthPoints;
    public bool isAggro;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected bool isElite;
    protected NavMeshAgent enemyNavMesh;
    [SerializeField]
    GameObject PickupHP, PickupEnergy;
    public int BaseEnergyPickupAmount = 10;
    

    private void Start()
    {
        HealthPoints = m_healthPoints;
    }

    public void TakeDamage(float amount)
    {
        HealthPoints -= amount;
    }

    private void Die()
    {
        PickupEnergy energyPickup = Instantiate(PickupEnergy, transform.position, Quaternion.identity).GetComponent<PickupEnergy>();
        if (isElite)
        {
            energyPickup.EnergyGainAmount = BaseEnergyPickupAmount * 2;
        }
        else
        {
            energyPickup.EnergyGainAmount = BaseEnergyPickupAmount;
        }
        Instantiate(PickupHP, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
