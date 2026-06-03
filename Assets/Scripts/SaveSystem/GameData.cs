using System;

[Serializable]
public class GameData
{
    public int souls;
    public SerializableDictionary<StatType, int> upgradePoints;

    public string selectedCharacterId;

    public GameData()
    {
        upgradePoints = new SerializableDictionary<StatType, int>();
        selectedCharacterId = "";
    }
}
