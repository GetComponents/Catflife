using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float MyDamage;

    [SerializeField]
    private float selfDestructTime;

    [SerializeField]
    private GameObject explosionVFX;

    private void Start()
    {
        Destroy(gameObject, selfDestructTime);
        //PlaySound AmbientFire ?
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(MyDamage);
<<<<<<< HEAD
            //PlaySound FirballHit
=======
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
>>>>>>> 55c02569fa54804bba564f1d7d7e601bda62d2f5
            Destroy(gameObject);
        }
        else if(other.tag == "Wall")
        {
<<<<<<< HEAD
            //PlaySound FirballHit
=======
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
>>>>>>> 55c02569fa54804bba564f1d7d7e601bda62d2f5
            Destroy(gameObject);
        }
        else if (other.tag == "Boss")
        {
            other.GetComponent<Boss>().TakeDamage(MyDamage);
            //PlaySound FirballHit
            Destroy(gameObject);
        }
    }
}
