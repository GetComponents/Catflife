using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnergy : MonoBehaviour
{
    [SerializeField]
    private float initialSpeed;
    public float mySpeed;
    public int EnergyGainAmount;
    PlayerInventory player;
    Transform playerCollision;
    Rigidbody rb;

    private void Start()
    {
        player = PlayerInventory.Instance;
        playerCollision = player.transform;
        rb = GetComponent<Rigidbody>();
        rb.AddForce((transform.position - player.transform.position).normalized * initialSpeed, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        rb.AddForce((playerCollision.position - transform.position).normalized * mySpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.GainEnergy(EnergyGainAmount);
            Destroy(gameObject);
        }
    }
}
