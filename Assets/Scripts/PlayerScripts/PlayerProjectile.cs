using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float MyDamage;
    private uint playingID;

    [SerializeField]
    private float selfDestructTime;

    [SerializeField]
    private GameObject explosionVFX;

    private void Start()
    {
        Destroy(gameObject, selfDestructTime);
        playingID = AkSoundEngine.PostEvent("Play_CharProjectileAmbience", this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(MyDamage);
            AkSoundEngine.PostEvent("Play_CharProjectileExplode", this.gameObject);
            AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
            Destroy(Instantiate(explosionVFX, transform.position, Quaternion.identity), 1);
            Destroy(gameObject);
        }
        else if(other.tag == "Wall")
        {
            AkSoundEngine.PostEvent("Play_CharProjectileExplode", this.gameObject);
            AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
            Destroy(Instantiate(explosionVFX, transform.position, Quaternion.identity), 1);
            Destroy(gameObject);
        }
        else if (other.tag == "Boss")
        {
            other.GetComponent<Boss>().TakeDamage(MyDamage);
            AkSoundEngine.PostEvent("Play_CharProjectileExplode", this.gameObject);
            AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
            Destroy(Instantiate(explosionVFX, transform.position, Quaternion.identity), 1);
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        AkSoundEngine.StopPlayingID(playingID, 200, AkCurveInterpolation.AkCurveInterpolation_Constant);
    }
}
