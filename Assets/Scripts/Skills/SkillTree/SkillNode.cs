using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillNode : MonoBehaviour
{
    [SerializeField] public InventoryManager inventoryManager;

    [SerializeField] public List<SkillNode> prerequisites = new List<SkillNode>();
    [SerializeField] public List<SkillNode> unlockers = new List<SkillNode>();

    [SerializeField] public int ranksUnlocked = 0;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
    }

    public abstract bool IsActiveSkillNode();
    public bool IsPassiveSkillNode() { return !IsActiveSkillNode(); }
    
    public virtual ActiveSkill GetActiveSkill() { return null; }
    public virtual PassiveSkill GetPassiveSkill() { return null; }

    public bool TryGetActiveSkill(out ActiveSkill activeSkill)
    {
        if (IsActiveSkillNode())
        {
            activeSkill = GetActiveSkill();
            return true;
        }
        activeSkill = null;
        return false;
    }

    public bool TryGetPassiveSkill(out PassiveSkill passiveSkill)
    {
        if (IsPassiveSkillNode())
        {
            passiveSkill = GetPassiveSkill();
            return true;
        }
        passiveSkill = null;
        return false;
    }

    public virtual void SkillIntoNode()
    {
        inventoryManager.TrySkillNode(this);
    }

    public virtual void SkillOutNode()
    {
        inventoryManager.TryUnskillNode(this);
    }
}
