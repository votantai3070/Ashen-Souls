using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void ShowTooltip(bool show, RectTransform target)
    {
        if (!show)
        {
            rectTransform.position = new Vector3(9999, 9999);
            return;
        }

        UpdatePosition(target);
    }

    private void UpdatePosition(RectTransform target)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0f;

        Vector2 targetPosition = target.position;

        targetPosition.x = targetPosition.x > screenCenterX
            ? targetPosition.x - offset.x
            : targetPosition.x + offset.x;

        float verticalHalfSize = rectTransform.sizeDelta.y / 2f;
        float topY = targetPosition.y + verticalHalfSize;
        float bottomY = targetPosition.y - verticalHalfSize;

        if (topY > screenTop)
            targetPosition.y = screenTop - verticalHalfSize - offset.y;
        else if (bottomY < screenBottom)
            targetPosition.y = bottomY + verticalHalfSize + offset.y;


        rectTransform.position = targetPosition;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>- {text} </color>";
    }
}
