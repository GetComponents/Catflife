using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField]
    GameObject watervfx;

    [SerializeField]
    Transform face;

    GameObject currentWater;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void StartWatering()
    {
        //PlaySound Watering
        currentWater = Instantiate(watervfx, face);
    }

    void EndWatering()
    {
        Destroy(currentWater);
        transform.position = startPos;
        transform.eulerAngles = new Vector3(0, 90, 0);
        GetComponent<Animator>().SetBool("IsWatering", false);
    }

    public void MoveWateringCan(Vector3 _plantPosition)
    {
        transform.position = _plantPosition + new Vector3(0, 1.8f, -2.8f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Animator>().SetBool("IsWatering", true);
    }
}
