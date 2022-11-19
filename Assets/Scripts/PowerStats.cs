using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

[Serializable]
public struct PowerStats : IEquatable<PowerStats>, IFormattable
{
    [SerializeField] public float baseValue; // B
    [SerializeField] public float baseAddition; // A
    [SerializeField] public float additiveMultiplier; // M
    [SerializeField] public float stackingMultiplier; // S
    [SerializeField] public float flatAddition; // F

    public float Value { get
        {
            float value = baseValue + baseAddition;
            value = (value * additiveMultiplier) + (value * stackingMultiplier);
            value += flatAddition;
            return value;
        } }

    public PowerStats(float baseValue = 1f, float baseAddition = 0f, float additiveMultiplier = 1f, float stackingMultiplier = 1f, float flatAddition = 0f)
    {
        this.baseValue = baseValue;
        this.baseAddition = baseAddition;
        this.additiveMultiplier = additiveMultiplier;
        this.stackingMultiplier = stackingMultiplier;
        this.flatAddition = flatAddition;
    }

    public void Reset(float baseValue)
    {
        this.baseValue = baseValue;
        baseAddition = 0f;
        additiveMultiplier = 1f;
        stackingMultiplier = 1f;
        flatAddition = 0f;
    }

    public bool Equals(PowerStats other)
    {
        return baseValue == other.baseValue &&
            baseAddition == other.baseAddition &&
            additiveMultiplier == other.additiveMultiplier &&
            stackingMultiplier == other.stackingMultiplier &&
            flatAddition == other.flatAddition;
    }

    public bool EqualsValue(PowerStats other, float threshold = 0.001f)
    {
        return Mathf.Abs(Value - other.Value) <= threshold;
    }

    /**
     * Formats the string to provide values of the stats.
     * B = base value,
     * A = base addition,
     * M = additive multiplier,
     * S = stacking multiplier,
     * F = flat addition,
     * V = calculated value
     */
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (String.IsNullOrEmpty(format)) format = "" + Value;
        if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

        switch (format.ToUpperInvariant())
        {
            case "B": return baseValue.ToString("0.00");
            case "A": return baseAddition.ToString("0.00");
            case "M": return additiveMultiplier.ToString("0.00");
            case "S": return stackingMultiplier.ToString("0.00");
            case "F": return flatAddition.ToString("0.00");
            case "V": return Value.ToString("0.00");
            default:
                throw new FormatException(String.Format("The {0} format string is not supported", format));
        }
    }
}
