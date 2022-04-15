using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCamera : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera plantCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            plantCamera.Priority = 11;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        plantCamera.Priority = 1;
    }
}
