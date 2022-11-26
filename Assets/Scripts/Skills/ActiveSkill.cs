using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "activeSkill", menuName = "Skills/Active Skills/Skill Name", order = 1)]
public abstract class ActiveSkill : PlayerSkill<ActiveSkill>
{
    [Header("Active Skill References")]
    [SerializeField] public Sprite skillSprite;
    [SerializeField] public bool delayToFixedUpdate = false;

    public bool UseSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        if (original)
        {
            Debug.LogError("Error - trying to use original ActiveSkill.UseSkill() of original ScriptableObject!\nUse ActiveSkill.GetCopy() before making this call.");
            return false;
        }

        // can't use skill if still on cooldown
        if (Time.fixedTime < skillAvailable)
            return false;

        if (!TriggerSkill(stationaryTransform, launchTransform, powerStats))
            return false;

        // update proper cooldown when using
        TriggerSkillCooldown();
        return true;
    }

    public abstract bool TriggerSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats);
    public virtual void AlignSkillImage(Image image)
    {
        image.sprite = skillSprite;
        image.SetNativeSize();
        image.rectTransform.rect.Set(image.rectTransform.rect.x, image.rectTransform.rect.y, image.rectTransform.rect.width * 3 / 2, image.rectTransform.rect.height * 3 / 2);
    }

    public void UpdateSkillImageFill(HudSkillIndicator indicator)
    {
        indicator.SetAbilityFill(skillAvailable, skillCooldown);
    }
}
