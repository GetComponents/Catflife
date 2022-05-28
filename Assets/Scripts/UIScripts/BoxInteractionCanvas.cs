using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxInteractionCanvas : MonoBehaviour
{
    public static BoxInteractionCanvas Instance;

    [SerializeField]
    GameObject myCanvas;

    [SerializeField]
    TextMeshProUGUI costText;

    Interactable boxToOpen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenCanvas(int _interactionCost, Interactable _boxToOpen)
    {
        costText.text = _interactionCost.ToString();
        boxToOpen = _boxToOpen;
        myCanvas.SetActive(true);
    }

    public void CancelInteraction()
    {
        myCanvas.SetActive(false);
    }

    public void ConfirmInteraction()
    {
        boxToOpen.UnpackItem();
        myCanvas.SetActive(false);
    }
}
