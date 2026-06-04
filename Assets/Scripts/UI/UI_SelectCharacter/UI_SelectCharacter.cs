using UnityEngine;
using UnityEngine.UI;

public class UI_SelectCharacter : MonoBehaviour, ISaveable
{
    private PlayerDataSO playerData;

    [SerializeField] private Button playButton;

    public UI_CharacterStat characterStatUI { get; private set; }

    private void Awake()
    {
        characterStatUI = GetComponentInChildren<UI_CharacterStat>(true);
    }

    private void Update()
    {
        playButton.interactable = CanPlay();
    }

    public void SetCharacterData(PlayerDataSO playerData)
    {
        this.playerData = playerData;
        characterStatUI.SetCharacterStatData(playerData);
    }

    private bool CanPlay() => playerData != null;

    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        if (playerData != null)
            data.selectedCharacterId = playerData.characterId;
    }
}
