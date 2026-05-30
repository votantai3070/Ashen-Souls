using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_UpgradePointSlot : MonoBehaviour
{
    [SerializeField] private Image connectorLine;
    [SerializeField] private Sprite connectorUnlockedSprite;
    [SerializeField] private Sprite connectorLockedSprite;

    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private bool isUnlocked;

    [SerializeField] private UI_UpgradePointSlot prerequisiteSkill;

    [Space]
    [SerializeField] private int pointCost = 1;

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

    public int GetPointCost() => pointCost;

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
        RefreshVisual();
    }

    public void Unlock()
    {
        if (!CanUnlock())
            return;

        GameManager.instance.MinusSouls(pointCost);
        UI.instance.upgradesUI.OnUpgradePointsInvoke();
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
        if (connectorLine != null)
        {
            connectorLine.sprite = isUnlocked ? connectorUnlockedSprite : connectorLockedSprite;
        }
    }

    private bool CanUnlock()
    {
        if (prerequisiteSkill != null && !prerequisiteSkill.IsUnlocked())
        {
            Debug.LogWarning($"{name}: Cannot unlock because prerequisite skill is not unlocked.", this);
            return false;
        }

        if (GameManager.instance.TotalSouls < pointCost)
        {
            Debug.LogWarning($"{name}: Not enough souls to unlock. Required: {pointCost}, Available: {GameManager.instance.TotalSouls}", this);
            return false;
        }

        return true;
    }
}