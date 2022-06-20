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
        if ((float)PlayerController.Instance.maxMana / 4 > ManaBarAmount)
        {
            UnlockNewManabar();
        }
        for (int i = 0; i < ManaBarAmount; i++)
        {
            //4 is the maxMana at 0 upgrades and always increases by 4
            barFillings[i].fillAmount = Mathf.Clamp((float)PlayerController.Instance.CurrentMana - (i * 4), 0, 4) / 4;
        }
    }

    public void UnlockNewManabar()
    {
        ManaBarAmount = Mathf.Clamp(PlayerInventory.Instance.ManaUpgrades + 1, 0, 4);
        manaBars[Mathf.Clamp(PlayerInventory.Instance.ManaUpgrades, 0, 3)].SetActive(true);
    }
}
