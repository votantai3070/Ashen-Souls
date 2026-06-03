using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "RPG Setup/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public string characterId;
    public string characterName;
    public RuntimeAnimatorController animator;
    public Sprite portrait;
    public GameObject weaponPrefab;

    [Space]
    public Stat_DataSO defaultCharacterStat;
}
