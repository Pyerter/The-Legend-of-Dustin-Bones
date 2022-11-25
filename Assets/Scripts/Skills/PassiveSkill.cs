using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "passiveSkill", menuName = "Skills/Passive Skill", order = 1)]
public abstract class PassiveSkill : PlayerSkill<PassiveSkill>
{
    [SerializeField] public bool isSubscribed = false;

    public bool TrySubscribePassive(PlayerSkillManager skillManager)
    {
        if (original)
        {
            Debug.LogError("Error - trying to use original PassiveSkill.SubscribeSkill() of original ScriptableObject!\nUse PassiveSkill.GetCopy() before making this call.");
            return false;
        }
        if (!isSubscribed)
        {
            SubscribePassive(skillManager);
            isSubscribed = true;
            return true;
        }
        return false;
    }

    public bool TryUnsubscribePassive(PlayerSkillManager skillManager)
    {
        if (isSubscribed)
        {
            UnsubscribePassive(skillManager);
            isSubscribed = false;
            return true;
        }
        return false;
    }

    public abstract void SubscribePassive(PlayerSkillManager skillManager);
    public abstract void UnsubscribePassive(PlayerSkillManager skillManager);
}
