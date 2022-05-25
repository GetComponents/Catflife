using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntProjectile : MonoBehaviour
{
    public float selfDestructTime;
    public int MyDamage;
    PlayerController player;
    private uint playingID;

    private void Start()
    {
        playingID = AkSoundEngine.PostEvent("Play_GruntProjectileThrow", this.gameObject);
        player = PlayerController.Instance;
        StartCoroutine(DieAfterTime());
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

    IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(selfDestructTime);
        PostImpactEvents();
        Destroy(gameObject);
    }

    private void PostImpactEvents()
    {
        AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
        AkSoundEngine.PostEvent("Play_GruntProjectileExplode", this.gameObject);
    }
}
