using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDescriptor : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI skillNameText;
    [SerializeField] public TextMeshProUGUI skillRankText;
    [SerializeField] public TextMeshProUGUI skillDescriptionText;
    [SerializeField] public string originalRankText;

    private void Awake()
    {
        originalRankText = skillRankText.text;
    }

    public void SetText(SkillNode node)
    {
        if (node.TryGetSkillDetails(out string nameText, out string rankText, out string descriptionText)) {
            skillNameText.text = nameText;
            skillRankText.text = originalRankText + " " + rankText;
            skillDescriptionText.text = descriptionText;
        }
    }
}
