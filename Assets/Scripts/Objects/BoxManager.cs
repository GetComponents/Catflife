using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;
    public List<Interactable> OpenedBoxes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Instance.LoadProgress();
            Destroy(gameObject);
            return;
        }
    }

    public void LoadProgress()
    {
    }
}
