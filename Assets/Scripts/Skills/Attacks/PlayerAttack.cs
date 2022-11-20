using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : TemporaryExistence
{
    [SerializeField] public float AttackValue { get; private set; }

    public void InitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        AttackValue = skill.valueByRank[skill.SkillRank] * powerStats.Value;
        OnInitializeAttack(skill, stationaryTransform, launchTransform, powerStats);
    }

    public abstract void OnInitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats);

    public override void OnDisappear()
    {
        base.OnDisappear();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (OnEnemyEnterTrigger(enemy))
            {
                OnHitEnemy(enemy);
            }
        }
    }
    public virtual bool OnEnemyEnterTrigger(Enemy enemy) { return false; }

    public virtual void OnTick() { }
    public virtual void OnHitEnemy(Enemy enemy) { }
    public virtual void OnHitSelf(PlayerController player) { }
}
