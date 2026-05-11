using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    [SerializeField] private float finalValue;
    private bool isDirty = true;

    public float GetValue()
    {
        finalValue = baseValue;

        if (isDirty)
        {
            finalValue = GetFinalValue();
            isDirty = false;
        }

        return finalValue;
    }

    public void AddModifier(float value, string sourceName, bool isPercent)
    {
        StatModifier modifier = new StatModifier(value, sourceName, isPercent);
        modifiers.Add(modifier);
        isDirty = true;
    }

    public void RemoveModifier(string sourceName)
    {
        modifiers.RemoveAll(modifier => modifier.sourceName == sourceName);
        isDirty = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;
        float percentBonus = 0f;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.isPercent)
                percentBonus += modifier.value;   //0.1 = 10%
            else
                finalValue += modifier.value;
        }

        //baseValue * %
        finalValue += baseValue * percentBonus;

        return finalValue;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}

[Serializable]
public class StatModifier
{
    public string sourceName;
    public float value;
    public bool isPercent;

    public StatModifier(float value, string sourceName, bool isPercent)
    {
        this.value = value;
        this.sourceName = sourceName;
        this.isPercent = isPercent;
    }
}
