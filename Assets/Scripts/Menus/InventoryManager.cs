using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] TextMeshProUGUI pointsLeftInidicator;
    [SerializeField] SkillDescriptor skillDescriptor;
    [SerializeField] public EventSystem eventSystem;

    [SerializeField] public List<SkillNode> unlockedSkills = new List<SkillNode>();
    [SerializeField] public ActiveSkillNodeSelector activeSkillSelector1;
    [SerializeField] public ActiveSkillNodeSelector activeSkillSelector2;
    [SerializeField] public ActiveSkillNodeSelector activeSkillSelector3;

    [SerializeField] public int skillPointsEarned = 3;
    [SerializeField] public int skillPointsUsed = 0;
    public int SkillPointsUsed { 
        get { return skillPointsUsed; } 
        set { 
            skillPointsUsed = value; 
            if (pointsLeftInidicator != null)
                pointsLeftInidicator.text = "Skill Points Left:\n" + GetSkillPointsLeft().ToString() + "/" + skillPointsEarned.ToString();
        } 
    }
    [SerializeField] public int skillPointsPerMastery = 3;
    [SerializeField] public int skillPointsPerUnlock = 3;

    [SerializeField] private ActiveSkillNodeSelector selectingActiveSkill = null;

    private void Awake()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        SkillPointsUsed = 0;
    }

    public int GetSkillPointsLeft()
    {
        return skillPointsEarned - skillPointsUsed;
    }

    public void GetSkillsByType(out List<ActiveSkill> activeSkills, out List<PassiveSkill> passiveSkills)
    {
        activeSkills = new List<ActiveSkill>();
        passiveSkills = new List<PassiveSkill>();

        foreach (SkillNode node in unlockedSkills)
        {
            if (node.TryGetActiveSkill(out ActiveSkill active))
                activeSkills.Add(active);
            else if (node.TryGetPassiveSkill(out PassiveSkill passive))
                passiveSkills.Add(passive);
        }
    }

    public List<ActiveSkill> GetAllActiveSkills()
    {
        List<ActiveSkill> activeSkills = new List<ActiveSkill>();
        foreach (SkillNode node in unlockedSkills)
        {
            if (node.TryGetActiveSkill(out ActiveSkill active))
                activeSkills.Add(active.GetCopy());
        }
        return activeSkills;
    }

    public List<PassiveSkill> GetAllPassiveSkills()
    {
        List<PassiveSkill> passiveSkills = new List<PassiveSkill>();
        foreach (SkillNode node in unlockedSkills)
        {
            if (node.TryGetPassiveSkill(out PassiveSkill passive))
                passiveSkills.Add(passive.GetCopy());
        }
        return passiveSkills;
    }

    public bool NodeIsUnlocked(SkillNode node)
    {
        bool haveUnlocks = node.unlockers.Count == 0;
        foreach (SkillNode unlocker in node.unlockers)
        {
            bool found = false;
            foreach (SkillNode unlocked in unlockedSkills)
            {
                if (unlocker == unlocked)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                haveUnlocks = true;
                break;
            }
        }
        if (!haveUnlocks)
            return false;

        bool havePrereqs = node.prerequisites.Count == 0;
        foreach (SkillNode prereq in node.prerequisites)
        {
            bool found = false;
            foreach (SkillNode unlocked in unlockedSkills)
            {
                if (prereq == unlocked)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }
        return true;
    }

    public bool CanSkillNode(SkillNode node, out int pointsUsed)
    {
        pointsUsed = 0;
        if (node.RanksUnlocked == 4)
        {
            pointsUsed = skillPointsPerMastery;
            return GetSkillPointsLeft() >= skillPointsPerMastery;
        }

        if (node.RanksUnlocked > 0)
        {
            pointsUsed = 1;
            return GetSkillPointsLeft() > 0;
        }

        if (node.RanksUnlocked == 0)
        {
            pointsUsed = skillPointsPerUnlock;
            return GetSkillPointsLeft() >= skillPointsPerUnlock && NodeIsUnlocked(node);
        }

        return false;
    }

    public bool TrySkillNode(SkillNode node)
    {
        if (selectingActiveSkill != null)
        {
            if (node.RanksUnlocked > 0 && node.TryGetActiveSkill(out ActiveSkill active))
            {
                SetActiveSkillSelection(selectingActiveSkill, active);
                selectingActiveSkill = null;
                return true;
            }
            selectingActiveSkill.SetActiveSkill(null);
            selectingActiveSkill = null;
            return false;
        }

        if (CanSkillNode(node, out int pointsRequired))
        {
            if (node.RanksUnlocked == 0)
            {
                unlockedSkills.Add(node);
            }
            node.RanksUnlocked += 1;
            SkillPointsUsed += pointsRequired;
            return true;
        }
        return false;
    }

    public bool CanUnskillNode(SkillNode node, out int pointsGained)
    {
        pointsGained = 0;
        if (node.RanksUnlocked == 5)
        {
            pointsGained = skillPointsPerMastery;
            return true;
        }

        if (node.RanksUnlocked > 1)
        {
            pointsGained = 1;
            return true;
        }

        if (node.RanksUnlocked == 1)
        {
            pointsGained = skillPointsPerUnlock;
            return true;
        }

        return false;
    }

    public bool TryUnskillNode(SkillNode node)
    {
        if (CanUnskillNode(node, out int pointsGained))
        {
            node.RanksUnlocked--;
            if (node.RanksUnlocked == 0)
                unlockedSkills.Remove(node);

            SkillPointsUsed -= pointsGained;
            return true;
        }
        return false;
    }

    public void UnlearnAllNodes()
    {
        SkillPointsUsed = 0;
        activeSkillSelector1.SetActiveSkill(null);
        activeSkillSelector2.SetActiveSkill(null);
        activeSkillSelector3.SetActiveSkill(null);
        for (int i = unlockedSkills.Count - 1; i >= 0; i--)
        {
            unlockedSkills[i].RanksUnlocked = 0;
            unlockedSkills.RemoveAt(i);
        }
    }

    public void BeginActiveSkillSelection(ActiveSkillNodeSelector selector)
    {
        if (selectingActiveSkill != null && selectingActiveSkill == selector)
        {
            // unselect a skill!
            selectingActiveSkill.SetActiveSkill(null);
            selectingActiveSkill = null;
        }
        else if (selectingActiveSkill != null)
        {
            // flip the skills!
            ActiveSkill temp = selector.activeSkill;
            selector.SetActiveSkill(selectingActiveSkill.activeSkill);
            selectingActiveSkill.SetActiveSkill(temp);
            selectingActiveSkill = null;
        }
        else
            selectingActiveSkill = selector;
    }

    public void SetActiveSkillSelection(ActiveSkillNodeSelector selector, ActiveSkill skill)
    {
        if (selector != activeSkillSelector1)
        {
            if (activeSkillSelector1.activeSkill != null && activeSkillSelector1.activeSkill.name.Equals(skill.name))
            {
                activeSkillSelector1.SetActiveSkill(null);
                selector.SetActiveSkill(skill);
                return;
            }
        }

        if (selector != activeSkillSelector2)
        {
            if (activeSkillSelector2.activeSkill != null && activeSkillSelector2.activeSkill.name.Equals(skill.name))
            {
                activeSkillSelector2.SetActiveSkill(null);
                selector.SetActiveSkill(skill);
                return;
            }
        }

        if (selector != activeSkillSelector3)
        {
            if (activeSkillSelector3.activeSkill != null && activeSkillSelector3.activeSkill.name.Equals(skill.name))
            {
                activeSkillSelector3.SetActiveSkill(null);
                selector.SetActiveSkill(skill);
                return;
            }
        }

        selector.SetActiveSkill(skill);
    }

    public void ApplySkillSelections()
    {
        player.skillManager.Skill1 = activeSkillSelector1.activeSkill;
        player.skillManager.Skill2 = activeSkillSelector2.activeSkill;
        player.skillManager.Skill3 = activeSkillSelector3.activeSkill;
        player.skillManager.autofireSkill1 = activeSkillSelector1.autoFireSkill;
        player.skillManager.autofireSkill2 = activeSkillSelector2.autoFireSkill;
        player.skillManager.autofireSkill3 = activeSkillSelector3.autoFireSkill;
        player.skillManager.pendingPassiveSkills = GetAllPassiveSkills();
        player.skillManager.UpdateSkillAssignments();
    }

    public void UpdateSkillDescription(SkillNode node)
    {
        skillDescriptor.SetText(node);
    }
}
