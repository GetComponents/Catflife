using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    GameObject swordReflect;

    //Does stuff if hits an enemy or an enemy projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && PlayerController.Instance.IsSwinging)
        {
            AkSoundEngine.PostEvent("Play_SwordHit", this.gameObject);
            Destroy(Instantiate(swordReflect, transform.position, Quaternion.identity), 2);
            other.GetComponent<Enemy>().TakeDamage(PlayerController.Instance.SwordDamage);
            PlayerController.Instance.CurrentMana += PlayerController.Instance.ManaGain;
        }
        else if (other.tag == "Enemy" && PlayerController.Instance.IsSpinning)
        {
            AkSoundEngine.PostEvent("Play_SwordHit", this.gameObject);
            Destroy(Instantiate(swordReflect, transform.position, Quaternion.identity), 2);
            other.GetComponent<Enemy>().TakeDamage(PlayerController.Instance.SwordDamage * PlayerController.Instance.SpinAttackDamageMultiplier);
        }
        else if (other.tag == "EnemyProjectile" && (PlayerController.Instance.IsSpinning || PlayerController.Instance.IsSwinging))
        {
            if (PlayerController.Instance.ReturnEnemyProjectile(other.transform))
            {
                Destroy(Instantiate(swordReflect, transform.position, Quaternion.identity), 2);
                AkSoundEngine.PostEvent("Play_SwordHit", this.gameObject);
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "Boss" && PlayerController.Instance.IsSwinging)
        {
            AkSoundEngine.PostEvent("Play_SwordHit", this.gameObject);
            Destroy(Instantiate(swordReflect, transform.position, Quaternion.identity), 2);
            other.GetComponent<Boss>().TakeDamage(PlayerController.Instance.SwordDamage);
            PlayerController.Instance.CurrentMana += PlayerController.Instance.ManaGain;
        }
        else if (other.tag == "Boss" && PlayerController.Instance.IsSpinning)
        {
            AkSoundEngine.PostEvent("Play_SwordHit", this.gameObject);
            Destroy(Instantiate(swordReflect, transform.position, Quaternion.identity), 2);
            other.GetComponent<Boss>().TakeDamage(PlayerController.Instance.SwordDamage * PlayerController.Instance.SpinAttackDamageMultiplier);
        }
    }
}
