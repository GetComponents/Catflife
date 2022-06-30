using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHP : MonoBehaviour
{
    public float SelfDestructTime;
    public float mySpeed;
    public int HealAmount;
    [SerializeField]
    float rotationSpeed;
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
        transform.eulerAngles += new Vector3(0, Time.deltaTime * rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AkSoundEngine.PostEvent("Play_HPJingle", this.gameObject);
            player.Heal(HealAmount);
            Destroy(gameObject);
        }
    }
}
