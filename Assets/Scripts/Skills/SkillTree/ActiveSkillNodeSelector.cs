using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ActiveSkillNodeSelector : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public InventoryManager inventoryManager;
    [SerializeField] public TextMeshProUGUI text;

    [SerializeField] public ActiveSkill activeSkill;

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
        if (text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void BeginActiveSkillSelection()
    {
        text.text = "...";
        inventoryManager.BeginActiveSkillSelection(this);
    }

    public void SetActiveSkill(ActiveSkill skill)
    {
        activeSkill = skill;
        if (skill != null)
        {
            text.text = skill.displayName;
        } else
        {
            text.text = "Nada";
        }
    }
}
