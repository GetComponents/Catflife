using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{

    //Rotates the Canvas to the camera of the player
    void Start()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation* Vector3.forward, Camera.main.transform.rotation* Vector3.up);
    }
}
