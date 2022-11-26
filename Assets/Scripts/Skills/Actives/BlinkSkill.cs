using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "blinkSkill", menuName = "Skills/Active Skills/Blink", order = 2)]
public class BlinkSkill : ActiveSkill
{
    public override bool TriggerSkill(Transform stationaryTransform, Transform launchTransform, PowerStats powerStats)
    {
        Vector3 position = stationaryTransform.position;
        Vector3 move = launchTransform.position - position;
        move.x *= 2;
        move.y *= 2;
        if (stationaryTransform.localScale.x < 0)
            move.x *= -1;
        position += move;
        stationaryTransform.position = position;
        return true;
    }
}
