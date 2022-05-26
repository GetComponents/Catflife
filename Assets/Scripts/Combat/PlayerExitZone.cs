using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExitZone : MonoBehaviour
{
    public bool IsAbleToLeave
    {
        get => m_isAbleToLeave;
        set
        {
            if (value == true && gateAnimator!= null)
            {
                gateAnimator.SetBool("openGate", true);
            }
            m_isAbleToLeave = value;
        }
    }
    bool m_isAbleToLeave;
    public bool IsExit;
    [SerializeField]
    Animator gateAnimator;

    private void Awake()
    {
        IsAbleToLeave = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && IsAbleToLeave)
        {
            AkSoundEngine.PostEvent("Play_LeavingFootsteps", this.gameObject);
            MapManager.Instance.PlayerTookExit = IsExit;
            SceneTransition.Instance.ChangeScene("Combat", 2);
        }
    }
}
