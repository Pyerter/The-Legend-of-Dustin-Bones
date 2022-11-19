using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "activeSkill", menuName = "Skills/Active Skills/Directed Projectile", order = 1)]
public class DirectedProjectile : ActiveSkill
{
    public override bool TriggerSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        GameObject obj = Instantiate(skillPrefab, stationaryTransform.position, launchTransform.rotation);
        if (obj.TryGetComponent<PlayerAttack>(out PlayerAttack pAttack))
        {
            pAttack.transform.localScale = Vector3.one;
            pAttack.gameObject.SetActive(true);
            Vector2 relativePosition = launchTransform.transform.localPosition;
            pAttack.StartAttack(relativePosition.normalized);
            return true;
        } else
        {
            Destroy(obj);
            Debug.LogError("Error creating DirectedProjectile - prefab did not have component PlayerAttack");
            return false;
        }
    }
}
