using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCamera : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera plantCamera;

    public void EnterPlantView()
    {
            plantCamera.Priority = 11;

    }

    public void ExitPlantView()
    {
        plantCamera.Priority = 1;

    }
}
