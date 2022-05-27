using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [HideInInspector]
    public UnityEvent OnEnergyChange;

    public int DashUpgrades
    {
        get => m_dashUpgrades;
        set
        {
            switch (value)
            {
                case 1:
                    PlayerController.Instance.Speed = speedProgression.x;
                    PlayerController.Instance.DashSpeed = dashProgression.x;
                    break;
                case 2:
                    PlayerController.Instance.Speed = speedProgression.y;
                    PlayerController.Instance.DashSpeed = dashProgression.y;
                    break;
                case 3:
                    PlayerController.Instance.Speed = speedProgression.z;
                    PlayerController.Instance.DashSpeed = dashProgression.z;
                    break;
                default:
                    break;
            }
            if (value > 3)
            {
                PlayerController.Instance.Speed = speedProgression.z;
                PlayerController.Instance.DashSpeed = dashProgression.z;
            }
            m_dashUpgrades = value;
        }
    }
    public int AttackUpgrades
    {
        get => m_attackUpgrades;
        set
        {
            switch (value)
            {
                case 1:
                    PlayerController.Instance.SwordDamage = attackProgression.x;
                    break;
                case 2:
                    PlayerController.Instance.SwordDamage = attackProgression.y;
                    break;
                case 3:
                    PlayerController.Instance.SwordDamage = attackProgression.z;
                    break;
                default:
                    break;
            }
            if (value > 3)
            {
                PlayerController.Instance.SwordDamage = attackProgression.z;
            }
            m_attackUpgrades = value;
        }
    }
    public int HPUpgrades
    {
        get => m_hpUpgrades;
        set
        {
            switch (value)
            {
                case 1:
                    PlayerController.Instance.MaxHP = (int)hpProgression.x;
                    break;
                case 2:
                    PlayerController.Instance.MaxHP = (int)hpProgression.y;
                    break;
                case 3:
                    PlayerController.Instance.MaxHP = (int)hpProgression.z;
                    break;
                default:
                    break;
            }
            if (value > 3)
            {
                PlayerController.Instance.MaxHP = (int)hpProgression.z;
            }
            m_hpUpgrades = value;
            PlayerController.Instance.HealthPoints = PlayerController.Instance.MaxHP;
        }
    }
    public int ManaUpgrades
    {
        get => m_manaUpgrades;
        set
        {
            switch (value)
            {
                case 1:
                    PlayerController.Instance.maxMana = (int)maxManaProgression.x;
                    PlayerController.Instance.ManaGain = (int)manaGainProgression.x;
                    break;
                case 2:
                    PlayerController.Instance.maxMana = (int)maxManaProgression.y;
                    PlayerController.Instance.ManaGain = (int)manaGainProgression.y;
                    break;
                case 3:
                    PlayerController.Instance.maxMana = (int)maxManaProgression.z;
                    PlayerController.Instance.ManaGain = (int)manaGainProgression.z;
                    break;
                default:
                    break;
            }
            if (value > 3)
            {
                PlayerController.Instance.maxMana = (int)maxManaProgression.z;
                PlayerController.Instance.ManaGain = (int)manaGainProgression.z;
            }
            m_manaUpgrades = value;
            PlayerController.Instance.CurrentMana = PlayerController.Instance.maxMana;
            //ManaBar.Instance.UnlockNewManabar();
        }
    }
    [SerializeField]
    private int m_dashUpgrades, m_attackUpgrades, m_hpUpgrades, m_manaUpgrades;

    [SerializeField]
    Vector3 dashProgression, speedProgression, attackProgression, hpProgression, maxManaProgression, manaGainProgression;
    public int Energy
    {
        get => m_energy;
        set
        {
            if (value != m_energy)
            {
                m_energy = value;
                OnEnergyChange?.Invoke();
            }
        }
    }
    [SerializeField]
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
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.transform.name);
    //}

    public void GainEnergy(int _amount)
    {
        Energy += _amount;
    }

    private void Start()
    {
        DashUpgrades = m_dashUpgrades;
        AttackUpgrades = m_attackUpgrades;
        HPUpgrades = m_hpUpgrades;
        ManaUpgrades = m_manaUpgrades;
    }
}
