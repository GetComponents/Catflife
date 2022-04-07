using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnpoint : MonoBehaviour
{
    private void Start()
    {
        PlayerInventory.Instance.transform.position = transform.position;
    }
}
