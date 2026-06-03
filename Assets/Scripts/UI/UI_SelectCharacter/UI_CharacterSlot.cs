using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSlot : MonoBehaviour
{
    private UI_SelectCharacter selectCharacterUI;

    [SerializeField] private PlayerDataSO characterData;
    [SerializeField] private Image characterIcon;

    private void Awake()
    {
        selectCharacterUI = GetComponentInParent<UI_SelectCharacter>();
    }

    private void Start()
    {
        SetCharacterIcon();
    }

    private void OnValidate()
    {
        gameObject.name = "UI_CharacterSlot - " + (characterData != null ? characterData.characterName : "None");
    }

    public void SetCharacterDataButton()
    {
        if (characterData == null)
            return;

        selectCharacterUI.SetCharacterData(characterData);
    }

    private void SetCharacterIcon()
    {
        if (characterData.portrait == null || characterIcon == null)
            return;

        characterIcon.sprite = characterData.portrait;
    }
}
