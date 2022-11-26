using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBombAttack : PlayerAttack
{
    [Header("References")]
    [SerializeField] Collider2D hitBox;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject startingLight;
    [SerializeField] GameObject explosionLight;
    [Header("Tweakables")]
    [SerializeField] protected Sprite secondPhaseSprite;
    [SerializeField] public float explosionDuration;
    [SerializeField] public float explosionRadiusMultiplier = 3f;
    [Header("Trackables")]
    [SerializeField] public bool exploding = false;
    [SerializeField] public List<Enemy> hitEnemies = new List<Enemy>();

    private void Awake()
    {
        if (hitBox == null)
        {
            hitBox = GetComponent<Collider2D>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        hitBox.enabled = false;
    }

    public override void OnDisappear()
    {
        if (exploding)
        {
            base.OnDisappear();
        } else
        {
            Explode();
        }
    }

    protected void Explode()
    {
        this.TimeDone = Time.fixedTime + explosionDuration;
        this.spriteRenderer.sprite = secondPhaseSprite;
        startingLight.SetActive(false);
        explosionLight.SetActive(true);
        exploding = true;
        hitBox.enabled = true;
        Vector3 scale = transform.localScale;
        scale.x *= explosionRadiusMultiplier;
        scale.y *= explosionRadiusMultiplier;
        transform.localScale = scale;
    }

    public override bool OnEnemyEnterTrigger(Enemy enemy)
    {
        if (exploding)
        {
            enemy.Health -= (int)AttackValue;
            return true;
        }
        return false;
    }

    public override bool OnEnemyRemainInTrigger(Enemy enemy)
    {
        return OnEnemyEnterTrigger(enemy);
    }

    public override void OnInitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        transform.localScale = Vector3.one;
        startingLight.SetActive(true);
        explosionLight.SetActive(false);
    }
}
