using UnityEngine;

public class PlayerData
{
    public string characterId;
    public string displayName;
    public RuntimeAnimatorController animator;
    public Sprite portrait;
    public GameObject weaponPrefab;

    [Space]
    public Stat_DataSO defaultCharacterStat;
}
