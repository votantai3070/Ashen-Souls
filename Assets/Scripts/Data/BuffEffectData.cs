using System;
using UnityEngine;

[Serializable]
public class BuffEffectData
{
    public StatType type;
    [Range(0f, 1f)]
    public float value;
}