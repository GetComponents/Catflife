using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlock : MonoBehaviour
{
    enum EUnlockType
    {
        NONE,
        SPINMOVE,
        TEST,
        TMP,
        FOO
    }

    [SerializeField]
    EUnlockType myUnlock;

    public void UnlockMyContent()
    {
        switch (myUnlock)
        {
            case EUnlockType.NONE:
                break;
            case EUnlockType.SPINMOVE:
                Debug.Log("spIiIiIiiIiiInnNNn");
                break;
            case EUnlockType.TEST:
                break;
            case EUnlockType.TMP:
                break;
            case EUnlockType.FOO:
                break;
            default:
                break;
        }
    }
}
