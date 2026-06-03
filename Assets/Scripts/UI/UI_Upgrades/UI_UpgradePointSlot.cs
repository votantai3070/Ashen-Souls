using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_UpgradePointSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    }

    private void Start()
    {
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
        if (!CanUnlock() || isUnlocked)
            return;

        GameManager.instance.MinusSouls(pointCost);
        UI.instance.upgradesUI.OnUpgradePointsInvoke();
        SetUnlocked(true);
    }

    public void Lock()
    {
        SetUnlocked(false);
    }

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
            connectorLine.sprite = isUnlocked ? connectorUnlockedSprite : connectorLockedSprite;
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

    public bool IsUnlocked() => isUnlocked;
    public int GetPointCost() => pointCost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUnlocked || pointCost <= 0)
            return;

        UI.instance.upgradeStatPointTooltip.ShowTooltip(true, (RectTransform)transform, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.instance.upgradeStatPointTooltip.ShowTooltip(false, (RectTransform)transform, this);
    }
}