using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PowerStats))]
public class PowerStatsPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20f * 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty baseValueProp = property.FindPropertyRelative("baseValue");
        SerializedProperty baseAdditionProp = property.FindPropertyRelative("baseAddition");
        SerializedProperty additiveMultProp = property.FindPropertyRelative("additiveMultiplier");
        SerializedProperty stackingMultProp = property.FindPropertyRelative("stackingMultiplier");
        SerializedProperty flatAdditionProp = property.FindPropertyRelative("flatAddition");

        EditorGUIUtility.wideMode = true;
        EditorGUIUtility.labelWidth = 200;
        position.height = 16;

        EditorGUI.indentLevel = 0;
        EditorGUI.LabelField(position, "Power Stats");
        position.y += position.height + 4;

        EditorGUI.indentLevel = 1;

        baseValueProp.floatValue = EditorGUI.FloatField(position, "Base Value", baseValueProp.floatValue);
        position.y += position.height + 4;

        baseAdditionProp.floatValue = EditorGUI.FloatField(position, "Base Addition", baseAdditionProp.floatValue);
        position.y += position.height + 4;

        additiveMultProp.floatValue = EditorGUI.FloatField(position, "Additive Multiplier", additiveMultProp.floatValue);
        position.y += position.height + 4;

        stackingMultProp.floatValue = EditorGUI.FloatField(position, "Stacking Multiplier", stackingMultProp.floatValue);
        position.y += position.height + 4;

        flatAdditionProp.floatValue = EditorGUI.FloatField(position, "Flat Addition", flatAdditionProp.floatValue);
        position.y += position.height + 4;
    }
}
