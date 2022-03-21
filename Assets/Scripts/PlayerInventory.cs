using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [SerializeField]
    private TextMeshProUGUI energyUI;

    public int DashUpgrades, AttackUpgrades, HPUpgrades, ManaUpgrades;

    [SerializeField]
    private int startEnergy;

    public int Energy
    {
        get => m_energy;
        set
        {
            if (value != m_energy)
            {
                energyUI.text = $"Energy: {value}";
                m_energy = value;
            }
        }
    }
    private int m_energy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Energy = startEnergy;
    }
}
