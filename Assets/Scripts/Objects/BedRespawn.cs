using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedRespawn : MonoBehaviour
{
    void Start()
    {
        PlayerInventory.Instance.transform.position = transform.position;
    }
}
