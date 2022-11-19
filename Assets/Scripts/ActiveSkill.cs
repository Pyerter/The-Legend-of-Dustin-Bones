using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "activeSkill", menuName = "Skills/Active Skill", order = 1)]
public abstract class ActiveSkill : ScriptableObject
{
    protected bool original = true;

    [Header("Tweakable Variables")]
    [SerializeField] public float skillCooldown = 0.1f;

    [Header("References")]
    [SerializeField] public GameObject skillPrefab;

    [Header("Trackable Variables")]
    [SerializeField] public float skillAvailable = 0.00f;

    public bool UseSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        if (original)
        {
            Debug.LogError("Error - trying to use original ActiveSkill.UseSkill() of original ScriptableObject!\nUse ActiveSkill.GetCopy() before making this call.");
            return false;
        }

        // can't use skill if still on cooldown
        if (Time.fixedTime < skillAvailable)
            return false;

        if (!TriggerSkill(stationaryTransform, launchTransform, powerStats))
            return false;

        // update proper cooldown when using
        skillAvailable = Time.fixedTime + skillCooldown;
        return true;
    }

    public ActiveSkill GetCopy()
    {
        ActiveSkill clone = Instantiate(this);
        clone.original = false;
        return clone;
    }

    public abstract bool TriggerSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats);
}
