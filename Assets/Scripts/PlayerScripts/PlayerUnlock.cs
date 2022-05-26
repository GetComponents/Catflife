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
        STATUE
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
                if (PlayerController.Instance.unlockedSpinMove)
                {
                    //PlaySound UnlockJingle
                    PlayerController.Instance.unlockedSpinMove = true;
                }
                break;
            case EUnlockType.PROJECTILE:
                if (PlayerController.Instance.unlockedProjectile)
                {
                    //PlaySound UnlockJingle
                    PlayerController.Instance.unlockedProjectile = true;
                }
                break;
            case EUnlockType.REFLECT:
                if (PlayerController.Instance.unlockedReflect)
                {
                    //PlaySound UnlockJingle
                    PlayerController.Instance.unlockedReflect = true;
                }
                break;
            case EUnlockType.STATUE:
                if (PlayerController.Instance.UnlockedStatue)
                {
                    //PlaySound UnlockJingle
                    PlayerController.Instance.UnlockedStatue = true;
                }
                break;
            default:
                break;
        }
    }
}
