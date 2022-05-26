using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCast : MonoBehaviour
{
    public float TimeUntilExplosion = 1;
    public float TimeUntilFaded = 1;
    public int MyDamage;

    [SerializeField]
    CapsuleCollider myCollider;

    [SerializeField]
    GameObject enemyToSpawn;

    private void Start()
    {
        StartCoroutine(Cast());
    }

    IEnumerator Cast()
    {
        //PlaySound MageCast 
        yield return new WaitForSeconds(TimeUntilExplosion);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        //PlaySound MageCastExplode
        myCollider.enabled = true;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        yield return new WaitForSeconds(TimeUntilFaded);
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(MyDamage);
        }
    }
}
