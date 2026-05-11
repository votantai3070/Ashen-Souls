using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCard : MonoBehaviour
{
    private UI ui;

    [SerializeField] private Skill_BaseSO skillData;
    [SerializeField] private Outline[] outlines;

    [SerializeField] private RectTransform cardRect;
    [SerializeField] private GameObject frontFace;
    [SerializeField] private UI_SkillCardBack backFace;
    [SerializeField] private float flipDuration = 0.4f;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    private void OnEnable()
    {
        Flip();
    }

    public void ChooseCard()
    {
        SkillProgressManager.instance.UnlockSkill(skillData);

        if (skillData is Skill_DataSO skill)
        {
            // Skill
            ui.player.skillManager.GetSkillByType(skill.skillType).SetSkillUpgrade(skill);
        }
        else if (skillData is SkillBuff_DataSO buff)
        {
            // Buff stat
            ui.player.stats.ApplyBuff(buff.skillStatData, buff.displayName, buff.isPercent);
        }

        ui.SwitchToIngameUI();
    }

    public void SetCardInfo(Skill_BaseSO skillData, string colorText)
    {
        outlines = GetComponentsInChildren<Outline>();
        this.skillData = skillData;

        backFace.SetupInfoCard(skillData, colorText);

        foreach (var outline in outlines)
        {
            outline.effectColor = HexToColor(colorText);
        }
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;

        Debug.LogWarning($"Invalid color string: {hex}");
        return Color.white;
    }

    public void Flip()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(cardRect.DOLocalRotate(new Vector3(0, 90, 0), flipDuration / 2f)
            .SetEase(Ease.InQuad));

        seq.AppendCallback(() =>
        {
            frontFace.SetActive(false);
            backFace.gameObject.SetActive(true);
            cardRect.localEulerAngles = new Vector3(0, -90, 0);
        });

        seq.Append(cardRect.DOLocalRotate(new Vector3(0, 0, 0), flipDuration / 2f)
            .SetEase(Ease.OutQuad));
    }

    public void ResetCard()
    {
        DOTween.Kill(cardRect);
        cardRect.localEulerAngles = Vector3.zero;
        frontFace.SetActive(true);
        backFace.gameObject.SetActive(false);
    }
}
