using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    public float currentDashCooldown;
    [SerializeField]
    float speed, maxSpeed, dashCooldown, dashSpeed;
    PlayerInput input;
    public LayerMask mask;

    [Space]
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    MeshRenderer myMeshRenderer;

    [Header("Health And Collision")]
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform playerHitBox;
    public int HealthPoints
    {
        get => m_healthPoints;
        set
        {
            if (value < 0)
            {
                m_healthPoints = 0;
            }
            else if (value < m_healthPoints)
            {
                StartCoroutine(TurnInvincible());
                m_healthPoints = value;
            }
        }
    }
    [SerializeField]
    private int m_healthPoints;
    [SerializeField]
    Material hurtMaterial;
    Material normalMaterial;
    public bool isInvincible;

    [Space]
    [SerializeField]
    Animator myAnimator;

    Vector2 m_moveDir = new Vector2();

    Vector3 dashDirection;

    bool dashing;


    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        HealthPoints = m_healthPoints;
        normalMaterial = myMeshRenderer.material;
        //input = new PlayerInput();
        //input.Player.Enable();
        //input.Player.Interact.performed += ChangeScene;
    }

    // Update is called once per frame
    void Update()
    {
        ReduceDashCooldown();
        Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 200))
        {
            Vector3 pointToLook = hit.point;
            playerHitBox.transform.LookAt(new Vector3(pointToLook.x, playerHitBox.transform.position.y, pointToLook.z), Vector3.up);
            //parent.transform.rotation = Quaternion.LookRotation(pointToLook - transform.position);
            //cube.position = pointToLook;
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!dashing)
            rb.AddForce(new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * speed, ForceMode.VelocityChange);
    }

    #region InputMethods
    public void Movement(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector2>();
    }

    public void MouseDown(InputAction.CallbackContext context)
    {
        myAnimator.SetBool("isSwinging", false);
        myAnimator.SetBool("isSwinging", true);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<float>());
        if (context.started)
        {
            ChangeScene(context);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (currentDashCooldown < 0)
        {
            dashing = true;
            currentDashCooldown = dashCooldown;
            myAnimator.SetBool("isDashing", true);
        }
    }
    #endregion

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

    private void ReduceDashCooldown()
    {
        currentDashCooldown -= Time.deltaTime;
    }

    public void HurtPlayer(int damage)
    {
        if (!isInvincible)
        {
            HealthPoints -= damage;
        }
    }

    IEnumerator TurnInvincible()
    {
        myMeshRenderer.material = hurtMaterial;
        isInvincible = true;
        yield return new WaitForSeconds(1);
        myMeshRenderer.material = normalMaterial;
        isInvincible = false;
    }

    #region AnimatorMethods
    private void StartSwing()
    {

    }

    private void EndSwing()
    {
        myAnimator.SetBool("isSwinging", false);
    }


    private void StartDash()
    {
        dashDirection = new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * dashSpeed;
        rb.AddForce(dashDirection, ForceMode.VelocityChange);
    }

    private void EndDash()
    {
        //rb.AddForce(-dashDirection, ForceMode.Force);
        myAnimator.SetBool("isDashing", false);
        dashing = false;
    }
    #endregion
}
