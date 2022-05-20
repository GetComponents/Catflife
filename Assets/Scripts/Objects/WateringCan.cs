using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField]
    GameObject watervfx;
    GameObject currentWater;

    void StartWatering()
    {
        //PlaySound Watering
        currentWater = Instantiate(watervfx, this.transform);
    }
    void EndWatering()
    {
        Destroy(currentWater);
        GetComponent<Animator>().SetBool("IsWatering", false);
    }
}
