using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCast : MonoBehaviour
{
    public float TimeUntilExplosion = 1;
    public float TimeUntilFaded = 1;
    public int MyDamage;

    [SerializeField]
    CapsuleCollider myCollider;

    private void Start()
    {
        StartCoroutine(Cast());
    }

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(TimeUntilExplosion);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        myCollider.enabled = true;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        yield return new WaitForSeconds(TimeUntilFaded);
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
