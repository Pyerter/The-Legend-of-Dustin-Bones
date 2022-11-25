using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour, PlayerControls.IInPlayActions
{
    PlayerControls controls;

    [Header("Tweakable Parameters")]
    [SerializeField] public float speed = 1f;
    [SerializeField] public float playerAttackRange = 2f;
    [SerializeField] public float iSeconds = 0.2f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public float shaderColorFadeLerp = 0.5f;
    [SerializeField] public float skillAutoFireThreshold = 0.5f;

    [Header("References")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform visionCone;
    [SerializeField] public Transform visionConeMask;
    [SerializeField] public Transform playerLookDirector;
    [SerializeField] public Material material;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public TextMeshProUGUI pauseTitle;
    [SerializeField] public Button resumeButton;
    [SerializeField] public PlayerSkillManager skillManager;

    [Header("Trackable Variables")]
    [SerializeField] public Vector3 movement = Vector3.zero;
    [SerializeField] public bool facingRight = true;
    [SerializeField] public float timeVulnerable = 0f;
    [SerializeField] private int currentHealth;
    [SerializeField] private float skill1AutoFire = 0f;
    [SerializeField] private float skill2AutoFire = 0f;
    [SerializeField] private float skill3AutoFire = 0f;

    // Properties
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; healthSlider.value = 1.0f * currentHealth / maxHealth; if (currentHealth <= 0) OnDeath(); } }
    

    private void Awake()
    {
        controls = new PlayerControls();
        controls.InPlay.SetCallbacks(this);

        //controls.InPlay.Movement.performed += (ctxt) => { OnMovement(ctxt); };

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        if (skillManager == null)
        {
            skillManager = GetComponentInChildren<PlayerSkillManager>();
        }
        if (material != null)
        {
            material.SetFloat("_Hurt", 0);
        }

        GameManager.Instance.OnPause += (s, b) =>
        {
            anim.speed = b ? 0 : 1; // if paused, set speed to 0, else normal 1
        };

        CurrentHealth = maxHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (Time.fixedTime > timeVulnerable)
            {
                CurrentHealth -= enemy.Damage;
                timeVulnerable = Time.fixedTime + iSeconds;
                material.SetFloat("_Hurt", 1);
            }
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.Paused)
        {
            return;
        }

        UpdateMovement();
        SetAnimVariables();
        CheckFacing();
        OrientVision();
        UpdateShaderVariables();
        UpdateActiveSkillInputs();
    }

    protected void UpdateMovement()
    {
        Vector3 position = transform.position;
        position += movement;
        transform.position = position;
    }

    protected void OrientVision()
    {
        if (movement.magnitude > 0)
        {
            float radAngle = Mathf.Atan2(movement.y, movement.x);
            float angle = Mathf.Rad2Deg * radAngle - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            visionCone.rotation = targetRotation;
            visionConeMask.rotation = targetRotation;

            playerLookDirector.transform.rotation = targetRotation;
            Vector2 pos = new Vector2(Mathf.Cos(radAngle) * playerAttackRange, Mathf.Sin(radAngle) * playerAttackRange);
            playerLookDirector.transform.localPosition = pos;
        }
    }

    protected void CheckFacing()
    {
        if ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
        {
            Flip();
        }
    }

    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    protected void SetAnimVariables()
    {
        if (movement.magnitude > 0)
        {
            if (Mathf.Abs(movement.x) > 0)
            {
                anim.SetInteger("DirectionGoing", 1);
            }
            else if (movement.y < 0)
            {
                anim.SetInteger("DirectionGoing", 0);
            }
            else if (movement.y > 0)
            {
                anim.SetInteger("DirectionGoing", 2);
            }
        }
        anim.SetFloat("Speed", movement.magnitude);
    }

    protected void UpdateShaderVariables()
    {
        float current = material.GetFloat("_Hurt");
        material.SetFloat("_Hurt", Mathf.Lerp(current, 0, shaderColorFadeLerp));
    }

    public void UpdateActiveSkillInputs()
    {
        if (skill1AutoFire != 0f && Time.fixedTime > skill1AutoFire)
            skillManager.UseSkill1(transform, playerLookDirector.transform);
        if (skill2AutoFire != 0f && Time.fixedTime > skill2AutoFire)
            skillManager.UseSkill2(transform, playerLookDirector.transform);
        if (skill3AutoFire != 0f && Time.fixedTime > skill3AutoFire)
            skillManager.UseSkill3(transform, playerLookDirector.transform);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        direction.Normalize();
        
        if (direction.x == 0 || direction.y == 0)
        {
            direction *= (speed * Time.fixedDeltaTime);
        } else
        {
            direction.x = Mathf.Sign(direction.x);
            direction.y = Mathf.Sign(direction.y);
            float componentSpeed = Mathf.Sqrt((speed * speed)/2);
            direction *= (componentSpeed * Time.fixedDeltaTime);
        }

        movement = direction;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!CanAct())
            return;
        //throw new System.NotImplementedException();
        if (context.started)
        {
            skillManager.UseSkill1(transform, playerLookDirector.transform);
            skill1AutoFire = Time.fixedTime + skillAutoFireThreshold;
        }
        if (context.performed && Time.fixedTime >= skill1AutoFire)
        {
            skillManager.UseSkill1(transform, playerLookDirector.transform);
        }
        if (context.canceled)
        {
            skill1AutoFire = 0f;
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (!CanAct())
            return;
        if (context.started)
        {
            skillManager.UseSkill2(transform, playerLookDirector.transform);
            skill2AutoFire = Time.fixedTime + skillAutoFireThreshold;
        }
        if (context.performed && Time.fixedTime >= skill2AutoFire)
        {
            skillManager.UseSkill2(transform, playerLookDirector.transform);
        }
        if (context.canceled)
        {
            skill2AutoFire = 0f;
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (!CanAct())
            return;
        if (context.started)
        {
            skillManager.UseSkill3(transform, playerLookDirector.transform);
            skill3AutoFire = Time.fixedTime + skillAutoFireThreshold;
        }
        if (context.performed && Time.fixedTime >= skill3AutoFire)
        {
            skillManager.UseSkill3(transform, playerLookDirector.transform);
        }
        if (context.canceled)
        {
            skill3AutoFire = 0f;
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started && CurrentHealth > 0)
        {
            bool repause = !GameManager.Instance.PauseMenuToggled && GameManager.Instance.Paused;
            GameManager.Instance.PauseMenuToggled = true;
            GameManager.Instance.TogglePause();
            if (repause)
                GameManager.Instance.TogglePause();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started && CurrentHealth > 0)
        {
            bool repause = GameManager.Instance.PauseMenuToggled && GameManager.Instance.Paused;
            GameManager.Instance.PauseMenuToggled = false;
            GameManager.Instance.TogglePause();
            if (repause)
                GameManager.Instance.TogglePause();
        }
    }

    public void OnDeath()
    {
        GameManager.Instance.PauseMenuToggled = true;
        GameManager.Instance.Paused = true;
        pauseTitle.text = "You've perished";
        resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

    public bool CanAct()
    {
        return !GameManager.Instance.Paused;
    }
}
