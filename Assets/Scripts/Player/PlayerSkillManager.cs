using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] public PlayerController player;

    [SerializeField] public PowerStats powerStats = new PowerStats();

    [SerializeField] public ActiveSkill skill1;
    [SerializeField] public ActiveSkill skill2;
    [SerializeField] public ActiveSkill skill3;

    [SerializeField] public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

    public delegate void OnHitEvent(PlayerController player, Enemy enemy);
    public event OnHitEvent OnHit;

    private void Awake()
    {
        if (player == null)
        {
            this.player = FindObjectOfType<PlayerController>();
        }

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

    private void FixedUpdate()
    {
        if (GameManager.Instance.Paused)
        {
            skill1?.DelaySkillAvailable();
            skill2?.DelaySkillAvailable();
            skill3?.DelaySkillAvailable();
            foreach (PassiveSkill skill in passiveSkills)
                skill.DelaySkillAvailable();
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

    public bool ApplyPassive(PassiveSkill passive)
    {
        if (passive.TrySubscribePassive(this))
        {
            if (passiveSkills.Contains(passive))
            {
                Debug.LogError("Subscribed passive skill while already in list");
                return false;
            }
            passiveSkills.Add(passive);
            return true;

        }
        return false;
    }

    public bool RemovePassive(PassiveSkill passive)
    {
        
        if (passive.TryUnsubscribePassive(this))
        {
            if (!passiveSkills.Contains(passive))
            {
                Debug.LogError("Unsubscribed passive skill while not in list");
                return false;
            }
            passiveSkills.Remove(passive);
            return true;
        }
        return false;
    }

    public void CallOnHit(Enemy enemy)
    {
        OnHit?.Invoke(player, enemy);
    }
}
