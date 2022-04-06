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
    }
}
