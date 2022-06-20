using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Watering : MonoBehaviour
{
    public static Watering Instance;

    [SerializeField]
    GameObject wateringCan;
    [SerializeField]
    Animator myAnimator;
    [SerializeField]
    GameObject EnterCanvas, ExitCanvas;

    [SerializeField]
    CinemachineVirtualCamera plantCamera, plantCameraUI;
    CinemachineVirtualCamera playerCamera, playerCameraUI;

    Camera myUICamera;

    //public bool HoldsTheCan;

    [SerializeField]
    GameObject attackPlantCanvas, manaPlantCanvas, hpPlantCanvas, speedPlantCanvas;

    [SerializeField]
    LayerMask layerMask;

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
    }

    private void Start()
    {
        FindCameras();
    }

    private void FindCameras()
    {
        playerCamera = PlayerCameras.Instance.mainCinCam;
        playerCameraUI = PlayerCameras.Instance.uiCinCam;
        myUICamera = PlayerCameras.Instance.uiCam;
    }

    public void EnableWatering()
    {
        if (myUICamera == null)
        {
            FindCameras();
        }
        myUICamera.enabled = false;
        StartCoroutine(EnableUI());
        EnterCanvas.SetActive(false);
        ExitCanvas.SetActive(true);
        PlayerController.Instance.gameObject.SetActive(false);
        plantCamera.Priority = 100;
        plantCameraUI.Priority = 100;
        //HoldsTheCan = true;
        attackPlantCanvas.SetActive(true);
        manaPlantCanvas.SetActive(true);
        hpPlantCanvas.SetActive(true);
        speedPlantCanvas.SetActive(true);
    }

    private IEnumerator EnableUI()
    {
        yield return new WaitForSeconds(2);
        myUICamera.enabled = true;
    }

    public void DisableWatering()
    {
        EnterCanvas.SetActive(true);
        ExitCanvas.SetActive(false);
        PlayerController.Instance.gameObject.SetActive(true);
        plantCamera.Priority = 0;
        plantCameraUI.Priority = 0;
        //HoldsTheCan = false;
        attackPlantCanvas.SetActive(false);
        manaPlantCanvas.SetActive(false);
        hpPlantCanvas.SetActive(false);
        speedPlantCanvas.SetActive(false);
        myUICamera.enabled = false;
        StartCoroutine(EnableUI());
        StartCoroutine(test());
    }

    /// <summary>
    /// fixes bug
    /// </summary>
    /// <returns></returns>
    private IEnumerator test()
    {
        playerCameraUI.Priority--;
        playerCamera.Priority--;
        yield return new WaitForEndOfFrame();
        playerCameraUI.Priority++;
        playerCamera.Priority++;
    }
}
