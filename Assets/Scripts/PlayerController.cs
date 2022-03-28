using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    Rigidbody rb;
    Vector3 AddedForce;
    [SerializeField]
    float speed, maxSpeed, dashCooldown;
    public float currentDashCooldown;
    PlayerInput input;
    public LayerMask mask;

    [SerializeField]
    Transform cube;

    Vector2 m_moveDir = new Vector2();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        //input = new PlayerInput();
        //input.Player.Enable();
        //input.Player.Interact.performed += ChangeScene;
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay,out hit, 200))
        {
            Vector3 pointToLook = hit.point;
            //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z),Vector3.up);
            //transform.rotation = Quaternion.LookRotation(transform.position - pointToLook);
            cube.position = pointToLook;
        }
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
        //Debug.Log(context.ReadValue<float>());
        if(context.started)
        {
            ChangeScene(context);
        }
    }

    private void ChangeScene(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene("EncounterSelection");
        }
        else
        {
            HideMap.Instance.ChangeMapState();
            SceneManager.UnloadSceneAsync("Combat");
        }
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
