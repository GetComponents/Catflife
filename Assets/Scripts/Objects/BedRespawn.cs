using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedRespawn : MonoBehaviour
{
    void Start()
    {
        //Debug.Log(transform.position);
        PlayerInventory.Instance.transform.position = transform.position;
    }
}
