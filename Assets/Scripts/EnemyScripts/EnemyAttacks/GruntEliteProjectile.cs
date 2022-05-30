using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEliteProjectile : MonoBehaviour
{
    public float mySpeed;
    public float selfDestructTime;
    [HideInInspector]
    public int MyDamage;
    PlayerController player;
    Transform playerCollision;
    Rigidbody rb;
    private uint playingID;

    private void Start()
    {
        playingID = AkSoundEngine.PostEvent("Play_GruntEliteProjectileThrow", this.gameObject);
        player = PlayerController.Instance;
        playerCollision = player.transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, selfDestructTime);
    }

    private void FixedUpdate()
    {
        rb.AddForce((playerCollision.position - transform.position).normalized * mySpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PostImpactEvents();
            player.TakeDamage(MyDamage);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Wall")
        {
            PostImpactEvents();
            Destroy(gameObject);
        }
    }
    
    private void PostImpactEvents()
    {
        AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
        AkSoundEngine.PostEvent("Play_GruntEliteProjectileExplode", this.gameObject);
    }

    private void OnDestroy()
    {
        AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
    }
}
