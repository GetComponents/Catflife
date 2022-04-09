using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlock : MonoBehaviour
{
    enum EUnlockType
    {
        NONE,
        SPINMOVE,
        PROJECTILE,
        REFLECT,
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
                PlayerController.Instance.unlockedSpinMove = true;
                break;
            case EUnlockType.PROJECTILE:
                PlayerController.Instance.unlockedProjectile = true;
                break;
            case EUnlockType.REFLECT:
                PlayerController.Instance.unlockedReflect = true;
                break;
            default:
                break;
        }
    }
}
