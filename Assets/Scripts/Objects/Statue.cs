using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{

    [SerializeField]
    GameObject projectile, slowingHitbox;

    private bool activated = false;
    private Enemy targetEnemy;

    private void Start()
    {
        if (!PlayerController.Instance.UnlockedStatue)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            if (!activated)
            {
                switch (PlayerController.Instance.LavalampColor)
                {
                    case 0:
                        ChangeTarget();
                        StartCoroutine(shootEnemies());
                        break;
                    case 1:
                        slowingHitbox.SetActive(true);
                        break;
                    default:
                        break;
                }
                activated = true;
            }
            Destroy(other.gameObject);
        }
    }


    IEnumerator shootEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (targetEnemy != null)
            {
                StatueProjectile tmp = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<StatueProjectile>();
                tmp.target = targetEnemy;
                tmp.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    private void ChangeTarget()
    {
        float shortestDistance = Mathf.Infinity;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.HealthPoints == 0)
            {
                continue;
            }
            float distanceToCurrentEnemy = Vector3.SqrMagnitude(enemy.transform.position - transform.position);
            if (distanceToCurrentEnemy < shortestDistance)
            {
                shortestDistance = distanceToCurrentEnemy;
                targetEnemy = enemy;
            }
        }
        if (targetEnemy != null)
            targetEnemy.OnDeath.AddListener(ChangeTarget);
    }
}
