using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntProjectile : MonoBehaviour
{
    public float selfDestructTime;
    public int MyDamage;
    PlayerController player;

    private void Start()
    {
        //PlaySound EnemyProjectileAmbient ?
        player = PlayerController.Instance;
        StartCoroutine(DieAfterTime());
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

    IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(selfDestructTime);
        //PlaySound EnemyProjectileExplode
        Destroy(gameObject);
    }
}
