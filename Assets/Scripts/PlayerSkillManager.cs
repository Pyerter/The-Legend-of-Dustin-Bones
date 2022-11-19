using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] public PowerStats powerStats = new PowerStats();

    [SerializeField] public ActiveSkill skill1;
    [SerializeField] public ActiveSkill skill2;
    [SerializeField] public ActiveSkill skill3;

    private void Awake()
    {
        if (skill1 != null)
        {
            skill1 = skill1.GetCopy();
        }
        if (skill2 != null)
        {
            skill2 = skill2.GetCopy();
        }
        if (skill3 != null)
        {
            skill3 = skill3.GetCopy();
        }
    }

    public bool UseSkill1(Transform stationaryTransform, Transform launchTransform)
    {
        return skill1 != null ? skill1.UseSkill(stationaryTransform, launchTransform, powerStats) : false;
    }

    public bool UseSkill2(Transform stationaryTransform, Transform launchTransform)
    {
        return skill2 != null ? skill2.UseSkill(stationaryTransform, launchTransform, powerStats) : false;
    }

    public bool UseSkill3(Transform stationaryTransform, Transform launchTransform)
    {
        return skill3 != null ? skill3.UseSkill(stationaryTransform, launchTransform, powerStats) : false;
    }
}
