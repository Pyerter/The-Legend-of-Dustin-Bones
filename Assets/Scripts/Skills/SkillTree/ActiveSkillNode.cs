using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillNode : SkillNode
{
    [SerializeField] ActiveSkill activeSkill;

    private void Awake()
    {
        if (activeSkill == null)
        {
            Debug.LogError("Error - active skill is not assigned!");
        }
    }

    public override bool IsActiveSkillNode()
    {
        return true;
    }

    public override ActiveSkill GetActiveSkill()
    {
        ActiveSkill skill = activeSkill.GetCopy();
        skill.SkillRank = RanksUnlocked;
        return skill;
    }
}
