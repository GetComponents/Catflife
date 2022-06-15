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
                myAnimator.SetBool("takeDamage", true);
                Die();
                StartCoroutine(TurnInvincible());
                m_healthPoints = 0;
            }
            else if (value < m_healthPoints)
            {
                StartCoroutine(TurnInvincible());
                myAnimator.SetBool("takeDamage", true);
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
    public GameObject Projectile;
    public float ProjectileDamage, ProjectileSpeed;
    Vector2 m_moveDir = new Vector2();
    [SerializeField]
    Transform castHand;
    Vector3 dashDirection;

    [HideInInspector]
    public bool IsDashing, DashStarted, IsSwinging, IsSpinning;
    [HideInInspector]
    public UnityEvent OnManaChange, OnHealthChange;

    float mouseContext;
    float walkingDistance;

    [Space]
    public int LavalampColor;
    private bool gameIsPaused;
    public bool IsInCombat = true;

    [Header("Player Enviroment Aware")]
    [SerializeField]
    AkGameObj _akGameObj;
    private string actualScene;

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
    }

    void Update()
    {
        if (!gameIsPaused)
        {
            ReduceDashCooldown();

            //Makes the player look to the mouse
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
        CurrentMana = maxMana;
        if (SceneManager.GetActiveScene().name == "CombatDebug" || SceneManager.GetActiveScene().name == "BossStage")
        {
            IsInCombat = true;
        }
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            rb.AddForce(new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * Speed, ForceMode.VelocityChange);
            if (m_moveDir != Vector2.zero)
            {
                walkingDistance += Time.deltaTime;
                if (walkingDistance >= 0.3f)
                {
                    if (SceneTransition.Instance.SceneToLoad != actualScene)
                    {
                        actualScene = SceneTransition.Instance.SceneToLoad;
                        SetSceneMaterialSound(actualScene);
                    }
                    AkSoundEngine.PostEvent("Play_Step", _akGameObj.gameObject);
                    walkingDistance = 0;
                }
            }
            //Walking blend. It is rotatet 45° because of the Camera
            if (playerHitBox.eulerAngles.y <= 315)
            {
                myAnimator.SetFloat("ForwardBlend", m_moveDir.y * ((Mathf.Abs(playerHitBox.eulerAngles.y - 135) - 90) / 90));
                myAnimator.SetFloat("RightBlend", m_moveDir.x * ((Mathf.Abs(playerHitBox.eulerAngles.y - 225) - 90) / 90));
            }
            else
            {
                myAnimator.SetFloat("ForwardBlend", m_moveDir.y * ((405 - playerHitBox.eulerAngles.y) / 90));
                myAnimator.SetFloat("RightBlend", m_moveDir.x * ((playerHitBox.eulerAngles.y - 315) / 90));
            }
        }
        //Starts the dash
        else if (DashStarted)
        {
            dashDirection = new Vector3((m_moveDir.y * -0.66f) + (m_moveDir.x * 0.66f), 0, (m_moveDir.y * 0.66f) + (m_moveDir.x * 0.66f)) * DashSpeed;
            rb.AddForce(dashDirection, ForceMode.VelocityChange);
            DashStarted = false;
        }
    }

    private void SetSceneMaterialSound(string _sceneToLoad)
    {
        if (_sceneToLoad.Equals("MainRoom"))
        {
            AkSoundEngine.SetSwitch("Material", "Default", this.gameObject);
        }
        else
        {
            AkSoundEngine.SetSwitch("Material", "Stone", this.gameObject);
        }
    }

    public void Die()
    {
        AkSoundEngine.PostEvent("Play_CharDeath", this.gameObject);
        myAnimator.SetBool("die", true);
        SceneTransition.Instance.ChangeScene("MainRoom", 0);
    }

    //Called when going to Main Room
    public void Revive()
    {
        HealthPoints = MaxHP;
        CurrentMana = maxMana;
        myAnimator.SetBool("die", false);
    }

    public void StartDamageAnim()
    {
        myAnimator.SetBool("takeDamage", false);
        myAnimator.SetBool("isDashing", false);
        myAnimator.SetBool("isSwinging", false);
        myAnimator.SetBool("isCasting", false);
    }

    private void ReduceDashCooldown()
    {
        currentDashCooldown -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            AkSoundEngine.PostEvent("Play_CharHurt", this.gameObject);
            HealthPoints -= damage;
        }
    }

    IEnumerator TurnInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(2);
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

    /// <summary>
    /// Called when the Sword hits an enemy projectile
    /// </summary>
    /// <param name="projectileTransform"></param>
    /// <returns></returns>
    public bool ReturnEnemyProjectile(Transform projectileTransform)
    {
        if (unlockedReflect)
        {
            GameObject tmp = Instantiate(Projectile, projectileTransform.position, Quaternion.identity);
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



    #region InputMethods
    public void Movement(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector2>();
        myAnimator.SetFloat("RightBlend", m_moveDir.x);
    }

    /// <summary>
    /// Swings the Sword. If mouse 1 is beig held, it charges
    /// </summary>
    /// <param name="context"></param>
    public void MouseDown(InputAction.CallbackContext context)
    {
        if (!gameIsPaused && IsInCombat)
        {
            mouseContext = context.ReadValue<float>();
            if ((myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walking")) && context.started)
            {
                myAnimator.SetBool("isSwinging", true);
            }
            else if (mouseContext == 0 && myAnimator.GetBool("isCharging"))
            {
                SpinAttack();
            }
        }
    }

    /// <summary>
    /// Called for the cheat console
    /// </summary>
    /// <param name="context"></param>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
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
        if (currentDashCooldown < 0 && context.started
            && (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walking")))
        {
            IsDashing = true;
            currentDashCooldown = dashCooldown;
            isInvincible = true;
            myAnimator.SetBool("isDashing", true);
        }
    }

    /// <summary>
    /// Fireball attack
    /// </summary>
    /// <param name="context"></param>
    public void Cast(InputAction.CallbackContext context)
    {
        if ((myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
            && context.started && unlockedProjectile && CurrentMana >= projectileManaCost)
        {
            if (IsInCombat)
            {

                CurrentMana -= projectileManaCost;
                myAnimator.SetBool("isCasting", true);
            }
        }
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    /// <param name="context"></param>
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

    

    #region AnimatorMethods
    public void StartSwing()
    {
        IsSwinging = true;
        AkSoundEngine.PostEvent("Play_SwordSwing", this.gameObject);
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
        AkSoundEngine.PostEvent("Play_CharDash", this.gameObject);
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
        AkSoundEngine.PostEvent("Play_CharSpin", this.gameObject);
    }

    public void EndSpinAttack()
    {
        IsSpinning = false;
        myAnimator.SetBool("isSwinging", false);
        myAnimator.SetBool("isSpinAttacking", false);
    }

    public void StartCast()
    {
        GameObject tmp = Instantiate(Projectile, castHand.position, Quaternion.identity);
        tmp.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward.normalized * ProjectileSpeed, ForceMode.Impulse);
        tmp.GetComponent<PlayerProjectile>().MyDamage = ProjectileDamage;
        AkSoundEngine.PostEvent("Play_CharProjectileThrow", this.gameObject);
    }

    public void EndCast()
    {
        myAnimator.SetBool("isCasting", false);
    }

    #endregion

   
}
