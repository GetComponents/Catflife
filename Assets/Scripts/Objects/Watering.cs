using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Watering : MonoBehaviour
{
    [SerializeField]
    GameObject wateringCan;
    [SerializeField]
    Animator myAnimator;


    public bool IsWatering;

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
    }


    void Update()
    {
        if (IsWatering)
        {
            Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit, 200, layerMask))
            {
                wateringCan.transform.position =  hit.point; 
            }
        }
    }
}
