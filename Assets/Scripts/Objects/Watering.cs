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
    [SerializeField]
    GameObject watervfx;
    GameObject currentWater;

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
                wateringCan.transform.eulerAngles = new Vector3(45, 0, 0);
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

    void StartWatering()
    {
        currentWater = Instantiate(watervfx, wateringCan.transform);
    }
    void EndWatering ()
    {
        Destroy(currentWater);
        myAnimator.SetBool("IsWatering", false);
    }

}
