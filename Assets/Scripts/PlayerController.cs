using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 AddedForce;
    [SerializeField]
    float speed, maxSpeed, dashCooldown;
    public float currentDashCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Dash();
    }

    private void Movement()
    {
        AddedForce = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            if (rb.velocity.z < maxSpeed)
                AddedForce.z += speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (rb.velocity.x > -maxSpeed)
                AddedForce.x += -speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (rb.velocity.z > -maxSpeed)
                AddedForce.z += -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x < maxSpeed)
                AddedForce.x += speed;
        }
        transform.TransformDirection(AddedForce);
        rb.AddForce(AddedForce);
    }

    private void Dash()
    {
        currentDashCooldown -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && currentDashCooldown <= 0)
        {
            currentDashCooldown = dashCooldown;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.transform.tag == "Interactable")
        {
            other.GetComponent<Interactable>().StartInteraction();
        }
    }
}
