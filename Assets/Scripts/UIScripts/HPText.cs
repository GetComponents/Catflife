using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    TextMeshProUGUI text;

    [SerializeField]
    Image Heart;

    [SerializeField]
    Sprite heartFull, heartHalf;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        PlayerController.Instance.OnHealthChange.AddListener(ChangeHPText);
        PlayerController.Instance.OnHealthChange.AddListener(ChangeHPDisplay);
        ChangeHPDisplay();
        ChangeHPText();
    }

    private void ChangeHPDisplay()
    {

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
