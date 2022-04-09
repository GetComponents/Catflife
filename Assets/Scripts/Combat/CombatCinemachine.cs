using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatCinemachine : MonoBehaviour
{
    public static CombatCinemachine Instance;

    public CinemachineVirtualCamera myCinemachine;
    public Camera myCam;

    private void Awake()
    {
        if (Instance == null)
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
