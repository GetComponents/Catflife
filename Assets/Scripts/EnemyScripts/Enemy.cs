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

    private void Start()
    {
        HealthPoints = m_healthPoints;
    }

    public void TakeDamage(float amount)
    {
        HealthPoints -= amount;
        //Debug.Log($"{gameObject.name} took {amount} dmg / ({HealthPoints} hp)");
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
