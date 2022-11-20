using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public List<SkillNode> unlockedSkills = new List<SkillNode>();

    [SerializeField] public int skillPointsEarned = 3;
    [SerializeField] public int skillPointsUsed = 0;
    [SerializeField] public int skillPointsPerMastery = 3;
    [SerializeField] public int skillPointsPerUnlock = 3;

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

    public List<PassiveSkill> GetPassiveSkills()
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
        if (node.ranksUnlocked == 4)
        {
            pointsUsed = skillPointsPerMastery;
            return GetSkillPointsLeft() >= skillPointsPerMastery;
        }

        if (node.ranksUnlocked > 0)
        {
            pointsUsed = 1;
            return GetSkillPointsLeft() > 0;
        }

        if (node.ranksUnlocked == 0)
        {
            pointsUsed = skillPointsPerUnlock;
            return GetSkillPointsLeft() >= skillPointsPerUnlock && NodeIsUnlocked(node);
        }

        return false;
    }

    public bool TrySkillNode(SkillNode node)
    {
        if (CanSkillNode(node, out int pointsRequired))
        {
            if (node.ranksUnlocked == 0)
            {
                unlockedSkills.Add(node);
            }
            node.ranksUnlocked += 1;
            skillPointsUsed += pointsRequired;
            return true;
        }
        return false;
    }

    public bool CanUnskillNode(SkillNode node, out int pointsGained)
    {
        pointsGained = 0;
        if (node.ranksUnlocked == 5)
        {
            pointsGained = skillPointsPerMastery;
            return true;
        }

        if (node.ranksUnlocked > 1)
        {
            pointsGained = 1;
            return true;
        }

        if (node.ranksUnlocked == 1)
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
            node.ranksUnlocked--;
            if (node.ranksUnlocked == 0)
                unlockedSkills.Remove(node);

            skillPointsUsed -= pointsGained;
            return true;
        }
        return false;
    }
}
