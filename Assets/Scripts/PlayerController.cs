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
    [SerializeField] public float attackCooldown = 0.75f;
    [SerializeField] public float iSeconds = 0.2f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public float shaderColorFadeLerp = 0.5f;

    [Header("References")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform visionCone;
    [SerializeField] public Transform visionConeMask;
    [SerializeField] public PlayerAttack playerAttack;
    [SerializeField] public Material material;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public TextMeshProUGUI pauseTitle;
    [SerializeField] public Button resumeButton;

    [Header("Trackable Variables")]
    [SerializeField] public Vector3 movement = Vector3.zero;
    [SerializeField] public bool facingRight = true;
    [SerializeField] public float attackAvailable = 0f;
    [SerializeField] public float timeVulnerable = 0f;
    [SerializeField] private int currentHealth;

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

            playerAttack.transform.rotation = targetRotation;
            Vector2 pos = new Vector2(Mathf.Cos(radAngle) * playerAttackRange, Mathf.Sin(radAngle) * playerAttackRange);
            //if (!facingRight)
            //pos.x *= -1;
            playerAttack.transform.localPosition = pos;
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

    public void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log("Doing movement");
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
        //throw new System.NotImplementedException();
        if (context.started && Time.fixedTime > attackAvailable && playerAttack != null)
        {
            //playerAttack.gameObject.SetActive(true);
            PlayerAttack pAttack = Instantiate(playerAttack, transform.position, playerAttack.transform.rotation);
            pAttack.transform.localScale = Vector3.one;
            pAttack.gameObject.SetActive(true);
            attackAvailable = Time.fixedTime + attackCooldown;
            Vector2 relativePosition = playerAttack.transform.localPosition;
            pAttack.StartAttack(relativePosition.normalized);
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (CurrentHealth > 0)
            GameManager.Instance.TogglePause();
    }

    public void OnDeath()
    {
        GameManager.Instance.Paused = true;
        pauseTitle.text = "You've perished";
        resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
}
