using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCardBack : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardInfoText;
    [SerializeField] private Image cardSkillImage;


    public void SetupInfoCard(Skill_BaseSO skillData, string colorText)
    {
        cardNameText.text = GetNameText(skillData, colorText);
        cardInfoText.text = GetDescriptionText(skillData);
        cardSkillImage.sprite = skillData.icon;
    }

    private string GetNameText(Skill_BaseSO skillData, string colorText)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<color={colorText}> {skillData.displayName}");

        return sb.ToString();
    }

    private string GetDescriptionText(Skill_BaseSO skillData)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine();
        sb.AppendLine($"{skillData.GetUpgradeDescription()}");

        return sb.ToString();
    }
}
