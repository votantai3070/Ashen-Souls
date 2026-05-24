using TMPro;
using UnityEngine;

public class UI_TotalSlot : MonoBehaviour
{
    public TextMeshProUGUI totalName;
    public TextMeshProUGUI totalValue;
    public TotalSummaryType totalType;

    private void OnValidate()
    {
        gameObject.name = $"Total Slot - {totalType}";
    }

    public void SetTotalValue(int value)
    {
        totalValue.text = value.ToString();
    }

    private void Start()
    {
        totalName.text = totalType.ToString();
    }
}
