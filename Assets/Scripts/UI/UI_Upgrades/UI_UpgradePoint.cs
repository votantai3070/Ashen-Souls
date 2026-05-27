using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_UpgradePoint : MonoBehaviour
{
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private bool isUnlocked;

    [SerializeField] private UI_UpgradePoint prerequisiteSkill;

    private Image upgradePointImage;

    private void Awake()
    {
        upgradePointImage = GetComponent<Image>();
        RefreshVisual();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (upgradePointImage == null)
            upgradePointImage = GetComponent<Image>();

        if (upgradePointImage != null)
            RefreshVisual();
    }
#endif

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
        RefreshVisual();
    }

    public void Unlock()
    {
        if (prerequisiteSkill != null && !prerequisiteSkill.IsUnlocked())
        {
            Debug.LogWarning($"{name}: Cannot unlock because prerequisite skill is not unlocked.", this);
            return;
        }
        SetUnlocked(true);
    }

    public void Lock()
    {
        SetUnlocked(false);
    }

    public bool IsUnlocked() => isUnlocked;

    private void RefreshVisual()
    {
        if (upgradePointImage == null)
        {
            Debug.LogWarning($"{name}: Missing Image component.", this);
            return;
        }

        Sprite targetSprite = isUnlocked ? unlockedSprite : lockedSprite;

        if (targetSprite == null)
        {
            Debug.LogWarning($"{name}: Missing {(isUnlocked ? "unlocked" : "locked")} sprite.", this);
            return;
        }

        upgradePointImage.sprite = targetSprite;
    }
}