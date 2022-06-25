using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupButton : MonoBehaviour
{
    public void ClosePopup()
    {
        Destroy(gameObject);
    }
}
