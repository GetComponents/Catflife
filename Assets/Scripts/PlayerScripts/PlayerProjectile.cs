using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float MyDamage;
    [SerializeField]
    private float selfDestructTime;

    private void Start()
    {
        Destroy(gameObject, selfDestructTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(MyDamage);
            Destroy(gameObject);
        }
        else if(other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
