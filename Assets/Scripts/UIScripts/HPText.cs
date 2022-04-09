using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HPText : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        PlayerController.Instance.OnHealthChange.AddListener(ChangeHPText);
        ChangeHPText();
    }
    private void ChangeHPText()
    {
        text.text = $"Health: {PlayerController.Instance.HealthPoints} / {PlayerController.Instance.MaxHP}";
    }

    private void OnDestroy()
    {
        PlayerInventory.Instance.OnEnergyChange.RemoveListener(ChangeHPText);
    }
}
