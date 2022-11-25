using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ActiveSkillNodeSelector : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public InventoryManager inventoryManager;
    [SerializeField] public TextMeshProUGUI skillText;
    [SerializeField] public TextMeshProUGUI autofireText;

    [SerializeField] public ActiveSkill activeSkill;
    [SerializeField] public bool autoFireSkill;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }
        if (skillText == null)
        {
            skillText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void BeginActiveSkillSelection()
    {
        skillText.text = "...";
        inventoryManager.BeginActiveSkillSelection(this);
    }

    public void SetActiveSkill(ActiveSkill skill)
    {
        activeSkill = skill;
        if (skill != null)
        {
            skillText.text = skill.displayName;
        } else
        {
            skillText.text = "Nada";
        }
    }

    public void ToggleAutofire()
    {
        autoFireSkill = !autoFireSkill;
        autofireText.text = autoFireSkill ? "A" : "X";
    }
}
