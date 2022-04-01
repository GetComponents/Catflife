using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float Strength;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && PlayerController.Instance.swinging)
        {
            other.GetComponent<Enemy>().TakeDamage(Strength);
        }
    }
}
