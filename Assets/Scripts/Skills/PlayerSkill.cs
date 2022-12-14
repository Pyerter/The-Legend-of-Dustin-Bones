using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill<S> : ScriptableObject where S : PlayerSkill<S>
{
    protected bool original = true;

    [Header("Tweakable Variables")]
    [SerializeField] public string displayName = "Cool Skill";
    [SerializeField] public string skillDescription = "Does cool things.";
    [SerializeField] public float skillCooldown = 0.1f;
    [SerializeField] public int maxRank = 5;
    [SerializeField] public float[] valueByRank = new float[5];

    [Header("References")]
    [SerializeField] public GameObject skillPrefab;

    [Header("Trackable Variables")]
    [SerializeField] public int skillRank = 1;
    [SerializeField] public int SkillRankIndex { get { return skillRank > 0 ? skillRank - 1 : 0; } }
    [SerializeField] public float CurrentSkillValue { get { return valueByRank[SkillRankIndex]; } }
    [SerializeField] public float skillAvailable = 0.00f;

    public int SkillRank { get { return skillRank; } set { if (value > 0 && value <= maxRank) skillRank = value; } }
    public bool HasMastery { get { return skillRank == maxRank; } }

    public S GetCopy()
    {
        S clone = (S)Instantiate(this);
        clone.original = false;
        return clone;
    }

    public void TriggerSkillCooldown()
    {
        skillAvailable = skillCooldown + Time.fixedTime;
    }

    public void DelaySkillAvailable()
    {
        skillAvailable += Time.fixedDeltaTime;
    }
}
