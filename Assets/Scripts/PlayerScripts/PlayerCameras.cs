using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameras : MonoBehaviour
{
    public static PlayerCameras Instance;

    public Camera mainCam, uiCam;
    public CinemachineVirtualCamera uiCinCam, mainCinCam;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
