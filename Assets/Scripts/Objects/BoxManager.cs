using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;
    public List<int> OpenedBoxesIndex;

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
        foreach (Interactable box in FindObjectsOfType<Interactable>())
        {
            if (OpenedBoxesIndex.Contains(box.BoxNumber) && box.myAction == EActionType.UNPACK)
            {
                box.OpenMyBox();
            }
        }
    }
}
