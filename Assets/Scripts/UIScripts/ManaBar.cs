using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public static ManaBar Instance;
    [SerializeField]
    Image barFilling;
    [SerializeField]
    GameObject myCanvas;

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
        PlayerController.Instance.OnManaChange.AddListener(changeMana);
    }

    private void changeMana()
    {
        barFilling.fillAmount = (float)PlayerController.Instance.CurrentMana / (float)PlayerController.Instance.maxMana;
    }
}
