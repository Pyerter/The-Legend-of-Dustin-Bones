using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "sharpBones", menuName = "Skills/Passive Skills/Sharp Bones", order = 1)]
public class SharpBonesPassive : PassiveSkill
{
    public override void SubscribePassive(PlayerSkillManager skillManager)
    {
        skillManager.powerStats.baseAddition += CurrentSkillValue; 
        if (HasMastery)
        {
            skillManager.powerStats.flatAddition += CurrentSkillValue;
        }
    }

    public override void UnsubscribePassive(PlayerSkillManager skillManager)
    {
        skillManager.powerStats.baseAddition -= CurrentSkillValue;
        if (HasMastery)
        {
            skillManager.powerStats.flatAddition -= CurrentSkillValue;
        }
    }
}
