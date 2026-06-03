using UnityEngine;

public class UI_SelectCharacter : MonoBehaviour, ISaveable
{
    private PlayerDataSO playerData;

    public UI_CharacterStat characterStatUI { get; private set; }

    private void Awake()
    {
        characterStatUI = GetComponentInChildren<UI_CharacterStat>(true);
    }

    public void SetCharacterData(PlayerDataSO playerData)
    {
        this.playerData = playerData;
        characterStatUI.SetCharacterStatData(playerData);
    }

    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        if (playerData != null)
            data.selectedCharacterId = playerData.characterId;
    }
}
