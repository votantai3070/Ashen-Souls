using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] private UI_StatSlot[] statSlots;

    private void Awake()
    {
        statSlots = GetComponentsInChildren<UI_StatSlot>(true);
    }

    //private void OnEnable()
    //{
    //    if (UI.instance?.player != null)
    //        UpdateStatUI();
    //}

    public void UpdateStatUI()
    {
        foreach (var stat in statSlots)
            stat.UpdateStatValue();
    }
}
