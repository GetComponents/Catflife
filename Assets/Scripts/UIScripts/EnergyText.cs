using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyText : MonoBehaviour
{
    TextMeshProUGUI text;
    void Start()
    {
        PlayerInventory.Instance.OnEnergyChange.AddListener(ChangeEnergyText);
        text = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeEnergyText()
    {
        Debug.Log(PlayerInventory.Instance.Energy);
        text.text = $"Energy: {PlayerInventory.Instance.Energy}";
    }

    private void OnDestroy()
    {
        PlayerInventory.Instance.OnEnergyChange.RemoveListener(ChangeEnergyText);
    }
}
