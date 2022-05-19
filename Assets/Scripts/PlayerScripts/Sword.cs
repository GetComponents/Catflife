using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && PlayerController.Instance.IsSwinging)
        {
            other.GetComponent<Enemy>().TakeDamage(PlayerController.Instance.SwordDamage);
            PlayerController.Instance.CurrentMana += PlayerController.Instance.ManaGain;
        }
        else if (other.tag == "Enemy" && PlayerController.Instance.IsSpinning)
        {
            other.GetComponent<Enemy>().TakeDamage(PlayerController.Instance.SwordDamage * PlayerController.Instance.SpinAttackDamageMultiplier);
        }
        else if (other.tag == "EnemyProjectile" && (PlayerController.Instance.IsSpinning || PlayerController.Instance.IsSwinging))
        {
            if (PlayerController.Instance.ReturnEnemyProjectile(other.transform))
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.tag == "Boss" && PlayerController.Instance.IsSwinging)
        {
            other.GetComponent<Boss>().TakeDamage(PlayerController.Instance.SwordDamage);
            PlayerController.Instance.CurrentMana += PlayerController.Instance.ManaGain;
        }
        else if (other.tag == "Boss" && PlayerController.Instance.IsSpinning)
        {
            other.GetComponent<Boss>().TakeDamage(PlayerController.Instance.SwordDamage * PlayerController.Instance.SpinAttackDamageMultiplier);
        }
    }
}
