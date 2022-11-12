using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 5;
    public int Health { get { return health; } set { health = value; if (health <= 0) KillEnemy(); } }

    [SerializeField]
    private int damage = 1;
    public int Damage { get { return damage; } private set { damage = value; } }

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private float controlTime = 0f;
    public float ControlTime { get { return controlTime; } set { controlTime = value; } }

    [SerializeField]
    protected float punGenerationChance = 0.1f;

    [SerializeField]
    public PlayerController target;

    private void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.Paused)
            return;

        MoveTowardsTarget();
    }

    protected virtual void MoveTowardsTarget()
    {
        if (Time.fixedTime < controlTime)
        {
            return;
        }
        Vector2 diff = target.transform.position - this.transform.position;
        diff.Normalize();
        float mult = Mathf.Sqrt((speed * speed * Time.fixedDeltaTime * Time.fixedDeltaTime) / 2);
        diff *= mult;
        Vector2 position = transform.position;
        position += diff;
        transform.position = position;
    }

    public void TryGeneratePun()
    {
        if (Random.Range(0, 1f) <= punGenerationChance)
        {
            // generate pun
            GameManager.Instance.GeneratePun(transform.position);
        }
    }

    public void KillEnemy()
    {
        TryGeneratePun();
        Destroy(gameObject);
    }

}
