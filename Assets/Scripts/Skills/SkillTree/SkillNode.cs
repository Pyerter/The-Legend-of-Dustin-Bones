using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class SkillNode : MonoBehaviour
{
    [SerializeField] public InventoryManager inventoryManager;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Button button;

    [SerializeField] public List<SkillNode> prerequisites = new List<SkillNode>();
    [SerializeField] public List<SkillNode> unlockers = new List<SkillNode>();

    [SerializeField] protected int ranksUnlocked = 0;
    [SerializeField] public int RanksUnlocked { get { return ranksUnlocked; } set { if (value <= 5 && value >= 0) { ranksUnlocked = value; if (text != null) text.text = ranksUnlocked.ToString(); } } }

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
        if (button == null)
        {
            button = GetComponent<Button>();
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
    public bool TryGetSkillDetails(out string nameText, out string rankText, out string descriptionText)
    {
        if (TryGetActiveSkill(out ActiveSkill activeSkill))
        {
            nameText = activeSkill.name;
            rankText = ranksUnlocked.ToString();
            descriptionText = activeSkill.skillDescription;
            return true;
        }
        if (TryGetPassiveSkill(out PassiveSkill passiveSkill))
        {
            nameText = passiveSkill.name;
            rankText = ranksUnlocked.ToString();
            descriptionText = passiveSkill.skillDescription;
            return true;
        }
        nameText = "Nada";
        rankText = "0";
        descriptionText = "...";
        return false;
    }

    public void UpdateSkillDescription()
    {
        inventoryManager.UpdateSkillDescription(this);
    }

    private void FixedUpdate()
    {
        if (inventoryManager.eventSystem.currentSelectedGameObject == button.gameObject)
        {
            UpdateSkillDescription();
        }
    }
}
