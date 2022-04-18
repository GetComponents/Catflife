using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInput : MonoBehaviour
{
    public static DebugInput Instance;
    [SerializeField]
    TMP_InputField myText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ConfirmInput()
    {
        DebugConsole.Instance.ReadInput(myText.text);
        myText.text = "";
    }
}
