using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// relays all animations to the main script
/// </summary>
public class PAnimationRelay : MonoBehaviour
{

    [SerializeField]
    PlayerController pc;
    private void StartSwing()
    {
        pc.StartSwing();
    }

    private void EndSwing()
    {
        pc.EndSwing();
    }

    private void StartDash()
    {
        pc.StartDash();
    }

    private void EndDash()
    {
        pc.EndDash();
    }

    private void StartSpinAttack()
    {
        pc.StartSpinAttack();
    }

    private void EndSpinAttack()
    {
        pc.EndSpinAttack();
    }

    private void StartCast()
    {
        pc.StartCast();
    }

    private void EndCast()
    {
        pc.EndCast();
    }

    private void StartTakeDamageAnim()
    {
        pc.StartDamageAnim();
    }
}
