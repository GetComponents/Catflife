using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueProjectile : MonoBehaviour
{
    [SerializeField]
    float speed, damage;
    [HideInInspector]
    public Enemy target;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target.OnDeath.AddListener(DestroyProjectile);
    }

    private void FixedUpdate()
    {
        rb.AddForce((target.transform.position - transform.position).normalized * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            target.TakeDamage(damage);
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
            Destroy(gameObject);
    }
}
