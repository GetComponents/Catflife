using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{

    [SerializeField]
    GameObject projectile, slowingHitbox, shootPoint;

    private bool activated = false;
    [SerializeField]
    private GameObject myMesh;
    private Enemy targetEnemy;

    private void Start()
    {
        //I dont use destroy GameObject because the logic of the statue is a child from the actual thing
        if (!PlayerController.Instance.UnlockedStatue)
        {
            Destroy(myMesh);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            if (!activated)
            {
                AkSoundEngine.PostEvent("Play_LightUpStatue", this.gameObject);
                //depending of the Light the player currently posseses, the statue does different stuff
                switch (PlayerController.Instance.LavalampColor)
                {
                    case 0:
                        ChangeTarget();
                        StartCoroutine(shootEnemies());
                        break;
                    case 1:
                        Instantiate(slowingHitbox, transform.position, Quaternion.identity);

                        break;
                    default:
                        break;
                }
                activated = true;
            }
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Shoots at enemies
    /// </summary>
    /// <returns></returns>
    IEnumerator shootEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (targetEnemy != null)
            {
                AkSoundEngine.PostEvent("Play_ShootStatue", this.gameObject);
                StatueProjectile tmp = Instantiate(projectile, shootPoint.transform.position, Quaternion.identity).GetComponent<StatueProjectile>();
                tmp.target = targetEnemy;
                tmp.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Called once the current target is destroyed and finds the closest enemy
    /// </summary>
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
