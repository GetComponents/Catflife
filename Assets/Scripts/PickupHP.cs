using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHP : MonoBehaviour
{
    public float SelfDestructTime;
    public float mySpeed;
    public int HealAmount;
    PlayerController player;
    Transform playerCollision;
    Rigidbody rb;

    private void Start()
    {
        player = PlayerController.Instance;
        playerCollision = player.transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, SelfDestructTime);
    }

    private void FixedUpdate()
    {
        rb.AddForce((transform.position - playerCollision.position).normalized * mySpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Heal(HealAmount);
            Destroy(gameObject);
        }
    }
}
