using UnityEngine;

public class UI_SelectCharacter : MonoBehaviour
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

        PlayerPrefs.SetString("SelectedCharacterId", playerData.characterId);
    }
}
