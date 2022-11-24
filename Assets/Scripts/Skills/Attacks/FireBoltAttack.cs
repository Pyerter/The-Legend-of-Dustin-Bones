using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltAttack : PlayerAttack
{
    [SerializeField]
    protected float moveSpeed = 5f;
    protected Vector3 flyingDirection = Vector2.zero;
    protected bool fired = false;

    public override bool OnEnemyEnterTrigger(Enemy enemy)
    {
        enemy.Health -= (int)AttackValue;
        return true;
    }

    public override void OnUpdate()
    {
        if (fired)
        {
            Vector3 position = transform.position;
            position += flyingDirection * Time.fixedDeltaTime * moveSpeed;
            transform.position = position;
        }
    }

    public override void OnInitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        transform.Rotate(new Vector3(0, 0, -90));
        transform.localScale = Vector3.one;
        Vector2 relativePosition = launchTransform.transform.localPosition;
        flyingDirection = relativePosition.normalized;
        fired = true;
    }
}
