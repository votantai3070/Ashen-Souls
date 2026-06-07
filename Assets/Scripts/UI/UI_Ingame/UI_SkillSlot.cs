using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    public Skill_DataSO skillData;

    [SerializeField] private Image skillImage;
    [SerializeField] private Image cooldownSkillImage;

    [Space]
    [SerializeField] private Sprite defaultSkillImage;

    private Coroutine cooldownCoroutine;

    public void SetSkillSlot(Skill_DataSO skillData)
    {
        if (skillData == null)
        {
            skillImage.sprite = defaultSkillImage;
            return;
        }

        this.skillData = skillData;
        skillImage.sprite = skillData.icon;
        cooldownSkillImage.fillAmount = 0;
    }

    public void ResetCooldown()
    {
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);
        cooldownSkillImage.fillAmount = 0f;
    }

    public void StartCooldown(float cooldownDuration)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        cooldownCoroutine = StartCoroutine(CooldownRoutine(cooldownDuration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        float elapsed = 0f;

        cooldownSkillImage.fillAmount = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownSkillImage.fillAmount = 1f - (elapsed / duration); // 1 → 0
            yield return null;
        }

        cooldownSkillImage.fillAmount = 0f;
        cooldownCoroutine = null;
    }
}
