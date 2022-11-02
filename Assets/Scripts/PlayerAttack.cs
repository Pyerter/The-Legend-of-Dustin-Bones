using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : TemporaryExistence
{
    [SerializeField]
    protected int damage = 5;
    public int Damage { get { return damage; } private set { damage = value; } }

    [SerializeField]
    protected float spinSpeed = 0.5f;
    [SerializeField]
    protected float moveSpeed = 5f;
    public bool spinning = false;
    protected Vector3 flyingDirection = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Health -= damage;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (spinning)
        {
            transform.Rotate(Vector3.forward, spinSpeed * Time.fixedDeltaTime);
            Vector3 position = transform.position;
            position += flyingDirection * Time.fixedDeltaTime * moveSpeed;
            transform.position = position;
        }
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Destroy(gameObject);
    }

    public void StartAttack(Vector2 direction)
    {
        spinning = true;
        flyingDirection = direction;
    }
}
