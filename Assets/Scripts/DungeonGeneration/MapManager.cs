using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField]
    GameObject map;
    [SerializeField]
    TextMeshProUGUI tmp;

    public bool PlayerTookExit;
    public Vector3 StagePos;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StagePos = Vector3.zero;
        PlayerTookExit = true;
    }
    public void ChangeMapState(bool _activate)
    {
        if (_activate)
        {
            if (PlayerTookExit)
            {
                PlayerInventory.Instance.transform.position = StagePos + new Vector3(-1, 1, 1);
            }
            else
            {
                PlayerInventory.Instance.transform.position = StagePos + new Vector3(1, 1, -1);
            }
            //PlayerController.Instance.gameObject.SetActive(false);
            DungeonGridGenerator.Instance.myLight.enabled = true;
            map.SetActive(true);
        }
        else
        {
            map.SetActive(false);
            PlayerController.Instance.gameObject.SetActive(true);
            DungeonGridGenerator.Instance.myLight.enabled = false;
        }
    }
}
