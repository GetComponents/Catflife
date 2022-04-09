using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlerProjectile : MonoBehaviour
{
    public int myDamage;
    [SerializeField]
    float timeUntilSelfdestruct;

    private void Start()
    {
        StartCoroutine(Selfdestruct());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(myDamage);
            Destroy(gameObject);
        }
        else if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Selfdestruct()
    {
        yield return new WaitForSeconds(timeUntilSelfdestruct);
        Destroy(gameObject);
    }
}
