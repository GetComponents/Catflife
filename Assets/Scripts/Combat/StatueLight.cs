using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatueLight : MonoBehaviour
{
    //slows enemies upon entering the radius
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.GetComponent<Enemy>() != null)
        {
            Enemy tmp = other.GetComponent<Enemy>();
            if (other.GetComponent<NavMeshAgent>() != null)
                other.GetComponent<NavMeshAgent>().speed = tmp.originalNavMeshSpeed * 0.5f;
            tmp.slowedSpeed = 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && GetComponent<Enemy>() != null)
        {
            Enemy tmp = other.GetComponent<Enemy>();
            if (other.GetComponent<NavMeshAgent>() != null)
                other.GetComponent<NavMeshAgent>().speed = tmp.originalNavMeshSpeed;
            tmp.slowedSpeed = 1;
        }
    }
}
