using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    public float currentDashCooldown;
    [SerializeField]
    float maxSpeed, dashCooldown;
    public float Speed, DashSpeed;
    public LayerMask GroundMask;

    [Header("Attacks")]
    [SerializeField]
    Sword mySword;
    public float SwordDamage;
    public float SpinAttackDamageMultiplier;


    [Space]
    [SerializeField]
    Camera mainCam;
    //[SerializeField]
    //MeshRenderer myMeshRenderer;

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
            if (value <= 0)
            {
                Die();
            }
            else if (value < m_healthPoints)
            {
                StartCoroutine(TurnInvincible());
                m_healthPoints = value;
            }
            else if (value > MaxHP)
            {
                m_healthPoints = MaxHP;
            }
            else
            {
                m_healthPoints = value;
            }
            OnHealthChange?.Invoke();
        }
    }
    [SerializeField]
    private int m_healthPoints;
    public int MaxHP;
    //[SerializeField]
    //Material hurtMaterial;
    //Material normalMaterial;
    public bool isInvincible;

    [Space]
    [SerializeField]
    Animator myAnimator;

    [Header("Mana and Abilities")]
    public int maxMana;
    public int ManaGain;
    [HideInInspector]
    public int CurrentMana
    {
        get => m_currentMana;
        set
        {
            if (value > maxMana)
            {
                m_currentMana = maxMana;
            }
            else
            {
                m_currentMana = value;
            }
            OnManaChange?.Invoke();
        }
    }
    int m_currentMana;
    public bool unlockedSpinMove, unlockedReflect, unlockedProjectile, UnlockedStatue;
    [SerializeField]
    int spinMoveManaCost, projectileManaCost;
    [SerializeField]
    GameObject projectile;
    public float ProjectileDamage, ProjectileSpeed;
    Vector2 m_moveDir = new Vector2();

    Vector3 dashDirection;

    [HideInInspector]
    public bool IsDashing, DashStarted, IsSwinging, IsSpinning;
    [HideInInspector]
    public UnityEvent OnManaChange, OnHealthChange;

    float mouseContext;

    [Space]
    public int LavalampColor;
    private bool gameIsPaused;

    //[SerializeField]
    //Animator displayPlayer;


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
        //normalMaterial = myMeshRenderer.material;
    }

    void Update()
    {
        if (!gameIsPaused)
        {
            ReduceDashCooldown();
            Ray cameraRay = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit, 200, GroundMask))
            {
                Vector3 pointToLook = hit.point;
                playerHitBox.transform.LookAt(new Vector3(pointToLook.x, playerHitBox.transform.position.y, pointToLook.z), Vector3.up);
            }
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
        CurrentMana = maxMana;
        //displayPlayer = GameObject.Find("Test").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
            rb.AddForce(new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * Speed, ForceMode.VelocityChange);
        else if (DashStarted)
        {
            dashDirection = new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * DashSpeed;
            rb.AddForce(dashDirection, ForceMode.VelocityChange);
            DashStarted = false;
        }
    }

    public void Die()
    {
        HealthPoints = MaxHP;
        SceneManager.LoadScene("SampleScene");
    }

    #region InputMethods
    public void Movement(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector2>();
        //Debug.Log(m_moveDir);
        //displayPlayer.SetFloat("ForwardBlend", m_moveDir.y);
        //displayPlayer.SetFloat("RightBlend", m_moveDir.x);
    }

    public void MouseDown(InputAction.CallbackContext context)
    {
        if (!gameIsPaused)
        {
            mouseContext = context.ReadValue<float>();
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && context.started)
            {
                myAnimator.SetBool("isSwinging", true);
            }
            else if (mouseContext == 0 && myAnimator.GetBool("isCharging"))
            {
                SpinAttack();
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //ChangeScene(context);
            if (DebugConsole.Instance != null)
            {
                if (!DebugConsole.Instance.myCanvas.activeInHierarchy)
                {
                    DebugConsole.Instance.OpenConsole();
                }
                else
                {
                    DebugInput.Instance.ConfirmInput();
                }
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (currentDashCooldown < 0 && context.started && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            IsDashing = true;
            currentDashCooldown = dashCooldown;
            isInvincible = true;
            myAnimator.SetBool("isDashing", true);
            // TODO PUT IN THE RIGHT PLACE
            AkSoundEngine.PostEvent("Play_Step", this.gameObject);
        }
    }

    public void Cast(InputAction.CallbackContext context)
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && context.started && unlockedProjectile && CurrentMana >= projectileManaCost)
        {
            CurrentMana -= projectileManaCost;
            myAnimator.SetBool("isCasting", true);
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            bool pausescreenIsOpen = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == "PauseScreen")
                {
                    pausescreenIsOpen = true;
                }
            }
            if (!pausescreenIsOpen)
            {
                SceneManager.LoadSceneAsync("PauseScreen", LoadSceneMode.Additive);
                gameIsPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                SceneManager.UnloadSceneAsync("PauseScreen");
                gameIsPaused = false;
                Time.timeScale = 1;
            }
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
            MapManager.Instance.ChangeMapState(true);
            SceneManager.UnloadSceneAsync("Combat");
        }
    }

    private void ReduceDashCooldown()
    {
        currentDashCooldown -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            HealthPoints -= damage;
        }
    }

    IEnumerator TurnInvincible()
    {
        //myMeshRenderer.material = hurtMaterial;
        isInvincible = true;
        yield return new WaitForSeconds(1);
        //myMeshRenderer.material = normalMaterial;
        isInvincible = false;
    }

    private void SpinAttack()
    {
        CurrentMana -= spinMoveManaCost;
        myAnimator.SetBool("isSpinAttacking", true);
        myAnimator.SetBool("isCharging", false);
    }

    public void HealFull()
    {
        HealthPoints = MaxHP;
    }

    public void Heal(int _amount)
    {
        HealthPoints += _amount;
    }

    #region AnimatorMethods
    public void StartSwing()
    {
        IsSwinging = true;
    }

    public void EndSwing()
    {
        IsSwinging = false;
        if (mouseContext != 0 && unlockedSpinMove && CurrentMana >= spinMoveManaCost)
        {
            myAnimator.SetBool("isCharging", true);
        }
        else
        {
            myAnimator.SetBool("isSwinging", false);
        }
    }

    public void StartDash()
    {
        DashStarted = true;
    }

    public void EndDash()
    {
        myAnimator.SetBool("isDashing", false);
        IsDashing = false;
        isInvincible = false;
    }

    public void StartSpinAttack()
    {
        IsSpinning = true;
    }

    public void EndSpinAttack()
    {
        IsSpinning = false;
        myAnimator.SetBool("isSwinging", false);
        myAnimator.SetBool("isSpinAttacking", false);
    }

    public void StartCast()
    {
        GameObject tmp = Instantiate(projectile, transform.GetChild(0).position, Quaternion.identity);
        tmp.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward.normalized * ProjectileSpeed, ForceMode.Impulse);
        tmp.GetComponent<PlayerProjectile>().MyDamage = ProjectileDamage;
    }

    public bool ReturnEnemyProjectile(Transform projectileTransform)
    {
        if (unlockedReflect)
        {
            GameObject tmp = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
            tmp.transform.localScale = projectileTransform.localScale;
            tmp.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward.normalized * ProjectileSpeed, ForceMode.Impulse);
            tmp.GetComponent<PlayerProjectile>().MyDamage = ProjectileDamage;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EndCast()
    {
        myAnimator.SetBool("isCasting", false);
    }
    #endregion
}
