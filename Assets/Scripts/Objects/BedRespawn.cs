using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedRespawn : MonoBehaviour
{
    void Start()
    {
        //Debug.Log(transform.position);
        PlayerInventory.Instance.transform.position = new Vector3(transform.position.x, transform.position.y +2, transform.position.z);
    }
}
