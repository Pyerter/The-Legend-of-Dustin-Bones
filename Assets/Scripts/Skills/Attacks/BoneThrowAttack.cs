using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneThrowAttack : PlayerAttack
{
    [SerializeField]
    protected float spinSpeed = 0.5f;
    [SerializeField]
    protected float moveSpeed = 5f;
    public bool spinning = false;
    protected Vector3 flyingDirection = Vector2.zero;

    public override bool OnEnemyEnterTrigger(Enemy enemy)
    {
        enemy.Health -= (int)AttackValue;
        return true;
    }

    public override void OnUpdate()
    {
        if (spinning)
        {
            transform.Rotate(Vector3.forward, spinSpeed * Time.fixedDeltaTime);
            Vector3 position = transform.position;
            position += flyingDirection * Time.fixedDeltaTime * moveSpeed;
            transform.position = position;
        }
    }

    public override void OnInitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        transform.localScale = Vector3.one;
        Vector2 relativePosition = launchTransform.transform.localPosition;
        spinning = true;
        flyingDirection = relativePosition.normalized;
    }
}
