using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Action onSkillSlotChange;

    public static UI instance { get; private set; }
    public Player player { get; private set; }

    public GameObject[] uiElements;
    public UI_Ingame ingameUI { get; private set; }
    public UI_SkillBoard skillBoardUI { get; private set; }

    private void Awake()
    {
        instance = this;

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        skillBoardUI = GetComponentInChildren<UI_SkillBoard>(true);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        onSkillSlotChange += ingameUI.skillHolder.SetupSkillSlots;
        ingameUI.skillHolder.SetupSkillSlots();
    }

    private void OnDestroy()
    {
        onSkillSlotChange -= ingameUI.skillHolder.SetupSkillSlots;
    }

    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            ControlsManager.instance.inputActions.Player.Disable();
        else
            ControlsManager.instance.inputActions.Player.Enable();
    }

    private void StopPlayerControlIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }

    private void SwitchTo(GameObject objectSwitching)
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        objectSwitching.SetActive(true);
    }

    public void OpenSkillBoard()
    {
        SwitchTo(skillBoardUI.gameObject);
        StopPlayerControlIfNeeded();
    }

    public void SwitchToIngameUI()
    {
        StopPlayerControls(false);
        SwitchTo(ingameUI.gameObject);
    }

    public void OnSkillSlotChange()
    {
        onSkillSlotChange?.Invoke();
    }
}
