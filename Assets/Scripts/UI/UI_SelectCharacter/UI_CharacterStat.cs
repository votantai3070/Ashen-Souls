using TMPro;
using UnityEngine;

public class UI_CharacterStat : MonoBehaviour
{
    private PlayerDataSO characterStatData;

    public UI_CharacterStatSlot[] characterStatSlots;
    [SerializeField] private TextMeshProUGUI characterStatText;


    private void Awake()
    {
        characterStatSlots = GetComponentsInChildren<UI_CharacterStatSlot>(true);
    }

    public void SetCharacterStatData(PlayerDataSO characterStatData)
    {
        Debug.Log($"SetCharacterStatData: {characterStatData.characterName}");
        this.characterStatData = characterStatData;
        UpdateStatUI();
    }

    public void UpdateCharacterStatName()
    {
        characterStatText.text = $"{characterStatData.characterName} Stats";
    }

    public void UpdateStatUI()
    {
        foreach (var stat in characterStatSlots)
            stat.UpdateStatValue(characterStatData.defaultCharacterStat);

        UpdateCharacterStatName();
    }
}
