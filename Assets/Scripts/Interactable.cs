using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    enum ETypeOfUpgrade
    {
        NONE,
        ATTACK,
        HP,
        MANA,
        SPEED
    }

    [SerializeField]
    ETypeOfUpgrade myUpgradeType;

    [SerializeField]
    private int costOfInteraction;

    PlayerInventory player;

    private void Start()
    {
        player = PlayerInventory.Instance;
    }

    public void StartInteraction()
    {
        if (costOfInteraction <= PlayerInventory.Instance.Energy)
        {
            player = PlayerInventory.Instance;
            processInteraction();
            player.Energy -= costOfInteraction;
        }
    }

    private void processInteraction()
    {
        if (myUpgradeType != ETypeOfUpgrade.NONE)
        {
            UpgradeStat();
        }
    }

    private void UpgradeStat()
    {
        switch (myUpgradeType)
        {
            case ETypeOfUpgrade.ATTACK:
                player.AttackUpgrades++;
                Debug.Log($"You Feel Stronger ({player.AttackUpgrades})");
                break;
            case ETypeOfUpgrade.HP:
                player.HPUpgrades++;
                Debug.Log($"You Feel Healthier ({player.HPUpgrades})");
                break;
            case ETypeOfUpgrade.MANA:
                player.ManaUpgrades++;
                Debug.Log($"You Feel More Resiliant ({player.ManaUpgrades})");
                break;
            case ETypeOfUpgrade.SPEED:
                player.DashUpgrades++;
                Debug.Log($"You Feel More Energetic ({player.DashUpgrades})");
                break;
        }
    }
}
