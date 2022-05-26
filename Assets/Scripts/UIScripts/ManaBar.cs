using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public static ManaBar Instance;


    [SerializeField]
    List<Image> barFillings;

    [SerializeField]
    List<GameObject> manaBars;

    [SerializeField]
    GameObject myCanvas;

    public int ManaBarAmount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(myCanvas);
        }
        else
        {
            Destroy(myCanvas);
            return;
        }
    }

    private void Start()
    {
        PlayerController.Instance.OnManaChange.AddListener(ChangeMana);
        UnlockNewManabar();
        ChangeMana();
    }

    private void ChangeMana()
    {
        for (int i = 0; i < ManaBarAmount; i++)
        {
            //4 ist Maxmana bei 0 Upgrades und jedes Upgrade gibt 4 extra Mana
            barFillings[i].fillAmount = Mathf.Clamp((float)PlayerController.Instance.CurrentMana - (i * 4), 0, 4) / 4;
        }
        //barFilling.fillAmount = (float)PlayerController.Instance.CurrentMana / (float)PlayerController.Instance.maxMana;
    }

    public void UnlockNewManabar()
    {
        ManaBarAmount = Mathf.Clamp(PlayerInventory.Instance.ManaUpgrades + 1, 0, 4);
        manaBars[Mathf.Clamp(PlayerInventory.Instance.ManaUpgrades, 0, 3)].SetActive(true);
    }
}
