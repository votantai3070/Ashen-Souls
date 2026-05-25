using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private float finalValue;
    private bool isDirty = true;

    public float GetValue()
    {
        if (isDirty)
        {
            finalValue = GetFinalValue();
            isDirty = false;
        }

        return finalValue;
    }

    public void SetBaseValue(float value)
    {
        if (Mathf.Approximately(baseValue, value))
            return;

        baseValue = value;
        isDirty = true;
    }

    public void UpdateModifier(float newValue, string sourceName, bool isPercent)
    {
        StatModifier existing = modifiers.Find(m => m.sourceName == sourceName);

        if (existing != null)
        {
            existing.value = newValue;
            existing.isPercent = isPercent;
        }
        else
        {
            AddModifier(newValue, sourceName, isPercent);
        }

        isDirty = true;
    }

    public void AddModifier(float value, string sourceName, bool isPercent)
    {
        modifiers.Add(new StatModifier(value, sourceName, isPercent));
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
                percentBonus += modifier.value;
            else
                finalValue += modifier.value;
        }

        finalValue += baseValue * percentBonus;
        return finalValue;
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
