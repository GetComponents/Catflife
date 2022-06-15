using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    [SerializeField]
    List<Image> Hearts;

    [SerializeField]
    Sprite heartFull, heartHalf, heartEmpty;

    private void Start()
    {
        PlayerController.Instance.OnHealthChange.AddListener(ChangeHPDisplay);
        ChangeHPDisplay();
    }

    /// <summary>
    /// Displays the HP as Sprites
    /// </summary>
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

    private void OnDestroy()
    {
        PlayerController.Instance.OnHealthChange.RemoveListener(ChangeHPDisplay);
    }
}
