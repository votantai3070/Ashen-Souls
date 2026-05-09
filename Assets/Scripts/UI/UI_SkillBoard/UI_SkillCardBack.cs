using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillCardBack : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardInfoText;


    public void SetupInfoCard(Skill_BaseSO skillData, string colorText)
    {
        if (skillData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        cardNameText.text = GetNameText(skillData, colorText);
        cardInfoText.text = GetDescriptionText(skillData);
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
        sb.AppendLine($"{skillData.description}");

        return sb.ToString();
    }


}
