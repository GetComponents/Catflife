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


    public bool HoldsTheCan;

    [SerializeField]
    LayerMask layerMask;

    Camera mainCam;

    private InputAction click;

    void Awake()
    {
        mainCam = Camera.main;
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.started += ctx => {
            if (IsWatering)
            {
                //PlaySound Watering
                myAnimator.SetBool("IsWatering", true);
            }
        };

        click.Enable();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        mainCam = Camera.main;       
    }


    void Update()
    {
        if (HoldsTheCan)
        {
            Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit, 200, layerMask))
            {
                wateringCan.transform.position =  hit.point;
            }
        }
    }

    public void EnableWatering()
    {
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.started += ctx => {
            if (HoldsTheCan)
            {
                myAnimator.SetBool("IsWatering", true);
            }
        };

        click.Enable();
        HoldsTheCan = true;
    }

    public void DisableWatering()
    {
        HoldsTheCan = false;
        click = null;
    }

    private void OnDestroy()
    {
        click = null;
    }
}
