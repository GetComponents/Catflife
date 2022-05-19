using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAggroRange : MonoBehaviour
{
    [SerializeField]
    Boss mainScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mainScript.PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            mainScript.PlayerIsInRange = false;
        }
    }
}
