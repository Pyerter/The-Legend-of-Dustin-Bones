using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudSkillIndicator : MonoBehaviour
{
    [SerializeField] public Image abilityImage;
    [SerializeField] public Slider abilityFill;

    public void Awake()
    {
        if (abilityImage == null)
        {
            abilityImage = GetComponentInChildren<Image>();
        }
        if (abilityFill == null)
        {
            abilityFill = GetComponentInChildren<Slider>();
        }
    }

    public void SetAbilityFill(float timeReady, float cooldown)
    {
        float val = (timeReady - Time.fixedTime) / cooldown;
        if (val < 0)
            val = 0;
        else if (val > 1)
            val = 1;
        abilityFill.value = val;
    }
}
