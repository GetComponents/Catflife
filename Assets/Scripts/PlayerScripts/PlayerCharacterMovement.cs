using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController characterContoller;
    [SerializeField]
    float gravity;
    Vector2 m_moveDir;


    public void Movement(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        characterContoller.Move(new Vector3(m_moveDir.x, gravity, m_moveDir.y));
    }
}
