using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "activeSkill", menuName = "Skills/Active Skills/Directed Projectile", order = 1)]
public class DirectedProjectile : ActiveSkill
{
    public override bool TriggerSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        GameObject obj = Instantiate(skillPrefab, stationaryTransform.position, launchTransform.rotation);
        if (obj.TryGetComponent<PlayerAttack>(out PlayerAttack pAttack))
        {
            obj.gameObject.SetActive(true);
            pAttack.InitializeAttack(this, stationaryTransform, launchTransform, powerStats);
            return true;
        } else
        {
            Destroy(obj);
            Debug.LogError("Error creating DirectedProjectile - prefab did not have component PlayerAttack");
            return false;
        }
    }


}
