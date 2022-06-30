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
        AkSoundEngine.PostEvent("Play_Watering", this.gameObject);
        currentWater = Instantiate(watervfx, face);
    }

    void EndWatering()
    {
        AkSoundEngine.PostEvent("Stop_Watering", this.gameObject);
        Destroy(currentWater);
        transform.position = startPos;
        transform.eulerAngles = new Vector3(0, 90, 0);
        GetComponent<Animator>().SetBool("IsWatering", false);
    }

    /// <summary>
    /// Starts the watering interaction
    /// </summary>
    /// <param name="_plantPosition"></param>
    public void MoveWateringCan(Vector3 _plantPosition)
    {
        transform.position = _plantPosition + new Vector3(0, 1.2f, -2f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Animator>().SetBool("IsWatering", true);
    }
}
