using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatCinemachine : MonoBehaviour
{
    public CinemachineVirtualCamera myCinemachine;
    public static CombatCinemachine Instance;
    public Camera myCam;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(myCam.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        myCinemachine.m_Follow = PlayerController.Instance.gameObject.transform.GetChild(0);
    }
}
