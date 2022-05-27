using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    //TextMeshProUGUI text;

    [SerializeField]
    List<Image> Hearts;

    [SerializeField]
    Sprite heartFull, heartHalf, heartEmpty;

    private void Start()
    {
        //text = GetComponent<TextMeshProUGUI>();
        //PlayerController.Instance.OnHealthChange.AddListener(ChangeHPText);
        PlayerController.Instance.OnHealthChange.AddListener(ChangeHPDisplay);
        ChangeHPDisplay();
        //ChangeHPText();
    }

    private void ChangeHPDisplay()
    {
        float healthAmount = PlayerController.Instance.HealthPoints;
        for (int i = 1; i <= PlayerController.Instance.MaxHP; i += 2)
        {
            if (i + 1 <= healthAmount)
            {
                Hearts[Mathf.FloorToInt(i / 2)].sprite = heartFull;
                continue;
            }
            if (i == healthAmount)
            {
                Hearts[Mathf.FloorToInt(i / 2)].sprite = heartHalf;
            }
            else
            {
                Hearts[Mathf.FloorToInt(i / 2)].sprite = heartEmpty;
            }
        }
    }

    //private void ChangeHPText()
    //{
    //    text.text = $"Health: {PlayerController.Instance.HealthPoints} / {PlayerController.Instance.MaxHP}";
    //}

    private void OnDestroy()
    {
        PlayerController.Instance.OnHealthChange.RemoveListener(ChangeHPDisplay);
    }
}
