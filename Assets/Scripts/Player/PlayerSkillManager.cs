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

    [SerializeField] public bool autofireSkill1;
    [SerializeField] public bool autofireSkill2;
    [SerializeField] public bool autofireSkill3;

    [SerializeField] public HudSkillIndicator skill1HudIndicator;
    [SerializeField] public HudSkillIndicator skill2HudIndicator;
    [SerializeField] public HudSkillIndicator skill3HudIndicator;

    [SerializeField] public List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
    [SerializeField] public List<PassiveSkill> pendingPassiveSkills = new List<PassiveSkill>();

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
        } else
        {
            skill1?.UpdateSkillImageFill(skill1HudIndicator);
            skill2?.UpdateSkillImageFill(skill2HudIndicator);
            skill3?.UpdateSkillImageFill(skill3HudIndicator);
        }
    }

    public void TryAutofires(Transform stationaryTransform, Transform launchTransform)
    {
        if (autofireSkill1)
            UseSkill1(stationaryTransform, launchTransform);
        if (autofireSkill2)
            UseSkill2(stationaryTransform, launchTransform);
        if (autofireSkill3)
            UseSkill3(stationaryTransform, launchTransform);
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
        } else
        {
            Debug.LogError("Failed to unsubscribe passive skill: " + passive.name);
        }
        return false;
    }

    public void UpdateSkillAssignments()
    {
        UpdateActiveSkillAssignments();
        UpdatePassiveSkillAssignments();
    }

    public void UpdateActiveSkillAssignments()
    {
        if (skill1 != null)
        {
            skill1HudIndicator.gameObject.SetActive(true);
            skill1.AlignSkillImage(skill1HudIndicator.abilityImage);
        }
        else
        {
            skill1HudIndicator.gameObject.SetActive(false);
        }

        if (skill2 != null)
        {
            skill2HudIndicator.gameObject.SetActive(true);
            skill2.AlignSkillImage(skill2HudIndicator.abilityImage);
        }
        else
        {
            skill2HudIndicator.gameObject.SetActive(false);
        }

        if (skill3 != null)
        {
            skill3HudIndicator.gameObject.SetActive(true);
            skill3.AlignSkillImage(skill3HudIndicator.abilityImage);
        }
        else
        {
            skill3HudIndicator.gameObject.SetActive(false);
        }
    }

    public void UpdatePassiveSkillAssignments()
    {
        for (int i = passiveSkills.Count - 1; i >= 0; i--)
        {
            PassiveSkill currentSkill = passiveSkills[i];
            PassiveSkill newSkill = null;
            foreach (PassiveSkill pendingSkill in pendingPassiveSkills)
            {
                if (pendingSkill.name.Equals(currentSkill.name))
                {
                    newSkill = pendingSkill;
                    break;
                }
            }
            if (newSkill != null && currentSkill.skillRank == newSkill.skillRank)
            {
                pendingPassiveSkills.Remove(newSkill);
            }
            else
            {
                RemovePassive(currentSkill);
            }
        }
        foreach (PassiveSkill skill in pendingPassiveSkills)
        {
            ApplyPassive(skill);
        }
        pendingPassiveSkills.Clear();
    }

    public void CallOnHit(Enemy enemy)
    {
        OnHit?.Invoke(player, enemy);
    }

    
}
