using System;

[Serializable]
public class GameData
{
    public int souls;
    public SerializableDictionary<StatType, int> upgradePoints;

    public GameData()
    {
        upgradePoints = new SerializableDictionary<StatType, int>();
    }
}
