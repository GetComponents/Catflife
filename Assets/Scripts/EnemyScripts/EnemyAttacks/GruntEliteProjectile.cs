using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEliteProjectile : MonoBehaviour
{
    public float mySpeed;
    public float selfDestructTime;
    [HideInInspector]
    public int MyDamage;
    PlayerController player;
    Transform playerCollision;
    Rigidbody rb;

    private void Start()
    {
        //PlaySound EnemyProjectileAmbient ?
        player = PlayerController.Instance;
        playerCollision = player.transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, selfDestructTime);
    }

    private void FixedUpdate()
    {
        rb.AddForce((playerCollision.position - transform.position).normalized * mySpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //PlaySound EnemyProjectileExplode
            player.TakeDamage(MyDamage);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Wall")
        {
            //PlaySound EnemyProjectileExplode
            Destroy(gameObject);
        }
    }
}
