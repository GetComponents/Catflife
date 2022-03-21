using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 AddedForce;
    [SerializeField]
    float speed, maxSpeed, dashCooldown;
    public float currentDashCooldown;

    Vector2 m_moveDir = new Vector2();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * speed, ForceMode.Force);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector2>();
    }

    public void MouseDown(InputAction.CallbackContext context)
    {
        
    }

    public void Interact(InputAction.CallbackContext context)
    {

    }

    private void Dash()
    {
        currentDashCooldown -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        //if (Input.GetKeyDown(KeyCode.E) && other.transform.tag == "Interactable")
        //{
        //    other.GetComponent<Interactable>().StartInteraction();
        //}
    }
}
