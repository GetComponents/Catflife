using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Watering : MonoBehaviour
{
    [SerializeField]
    GameObject wateringCan;

    public bool IsWatering;

    [SerializeField]
    LayerMask layerMask;

    Camera mainCam;

    private InputAction click;

    void Awake()
    {
        mainCam = Camera.main;
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.performed += ctx => {
            if (IsWatering)
            {
                wateringCan.transform.eulerAngles = new Vector3(45, 0, 0);
            }
        };
        click.canceled += ctx =>
        {
            if (IsWatering)
            {
                wateringCan.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        };
        click.Enable();
    }

    private void Start()
    {
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
