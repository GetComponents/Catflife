using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HideMap : MonoBehaviour
{
    public static HideMap Instance;
    bool isOn = true;
    [SerializeField]
    GameObject map;
    [SerializeField]
    TextMeshProUGUI tmp;

    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeMapState()
    {
        if (isOn)
        {
            map.SetActive(false);
            tmp.text = "Enable Map";
        }
        else
        {
            map.SetActive(true);
            tmp.text = "Disable Map";
        }
        isOn = !isOn;
    }
}
