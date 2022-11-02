using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IInPlayActions
{
    PlayerControls controls;

    [Header("Tweakable Parameters")]
    [SerializeField] public float speed = 1f;
    [SerializeField] public float playerAttackRange = 2f;

    [Header("References")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform visionCone;
    [SerializeField] public Transform visionConeMask;
    [SerializeField] public PlayerAttack playerAttack;

    [Header("Trackable Variables")]
    [SerializeField] public Vector3 movement = Vector3.zero;
    [SerializeField] public bool facingRight = true;

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
            if (!facingRight)
                pos.x *= -1;
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
        if (playerAttack != null)
        {
            playerAttack.gameObject.SetActive(true);
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
    }
}
