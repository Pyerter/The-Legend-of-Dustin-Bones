using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBall : PlayerAttack
{
    [SerializeField] public Light2D light;
    [SerializeField] public SpriteRenderer lightSprite;
    [SerializeField] public float initialOuterRadius;
    [SerializeField] public float endRadiusPercent = 0.5f;
    [SerializeField] public float endRadius;
    [SerializeField] public Color initialColor;
    [SerializeField] public Color endColor;
    public override void OnInitializeAttack(ActiveSkill skill, Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        initialOuterRadius = light.pointLightOuterRadius;
        endRadius = initialOuterRadius * endRadiusPercent;
        initialColor = light.color;
        lightSprite = GetComponent<SpriteRenderer>();
        initialColor = lightSprite.color;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        float percentDone = 1 - (TimeDone - Time.fixedTime) / Duration;
        float radius = Mathf.Lerp(initialOuterRadius, endRadius, percentDone);
        lightSprite.color = Color.Lerp(initialColor, endColor, percentDone);
        light.pointLightOuterRadius = radius;
    }
}
