using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillNode : SkillNode
{
    [SerializeField] PassiveSkill passiveSkill;

    private void Awake()
    {
        if (passiveSkill == null)
        {
            Debug.LogError("Error - passive skill is not assigned!");
        }
    }

    public override bool IsActiveSkillNode()
    {
        return false;
    }

    public override PassiveSkill GetPassiveSkill()
    {
        return passiveSkill;
    }
}
