using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantInteractionCanvas : MonoBehaviour
{
    public static PlantInteractionCanvas Instance;

    [SerializeField]
    GameObject myCanvas, attackText, hpText, manaText, speedText;

    [SerializeField]
    TextMeshProUGUI costText;

    Interactable plantToWater;

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

    /// <summary>
    /// Opens a Menu depending on what is supposed to get upgraded
    /// </summary>
    /// <param name="_interactionCost"></param>
    /// <param name="_plantToWater"></param>
    public void OpenCanvas(int _interactionCost, Interactable _plantToWater)
    {
        costText.text = _interactionCost.ToString();
        plantToWater = _plantToWater;
        myCanvas.SetActive(true);
        switch (_plantToWater.myUpgradeType)
        {
            case ETypeOfUpgrade.NONE:
                break;
            case ETypeOfUpgrade.ATTACK:
                attackText.SetActive(true);
                break;
            case ETypeOfUpgrade.HP:
                hpText.SetActive(true);
                break;
            case ETypeOfUpgrade.MANA:
                manaText.SetActive(true);
                break;
            case ETypeOfUpgrade.SPEED:
                speedText.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CancelInteraction()
    {
        myCanvas.SetActive(false);
        speedText.SetActive(false);
        manaText.SetActive(false);
        attackText.SetActive(false);
        hpText.SetActive(false);
        
    }

    public void ConfirmInteraction()
    {
        plantToWater.UpgradeStat();
        myCanvas.SetActive(false);
        speedText.SetActive(false);
        manaText.SetActive(false);
        attackText.SetActive(false);
        hpText.SetActive(false);
    }
}
