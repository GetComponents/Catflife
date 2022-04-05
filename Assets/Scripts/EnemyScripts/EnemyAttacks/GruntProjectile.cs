using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntProjectile : MonoBehaviour
{
    public float selfDestructTime;
    [HideInInspector]
    public int MyDamage;
    PlayerController player;

    private void Start()
    {
        player = PlayerController.Instance;
        StartCoroutine(DieAfterTime());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.TakeDamage(MyDamage);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }
}
