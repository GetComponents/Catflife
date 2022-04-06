using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererArm : MonoBehaviour
{
    public int myDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(myDamage);
        }
    }
}
