using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, IOpenUI
{
    public static UI instance { get; private set; }

    public event Action OnSkillSlotChange;
    public event Action OnPlayerSet;

    private Button[] buttons;
    public Player player { get; private set; }

    public GameObject[] uiElements;

    #region UI Screens
    public UI_Ingame ingameUI { get; private set; }
    public UI_SkillBoard skillBoardUI { get; private set; }
    public UI_Stats statsUI { get; private set; }
    public UI_TotalSummary totalSummaryUI { get; private set; }
    public UI_Settings settingsUI { get; private set; }
    public UI_FadeScreen fadeUI { get; private set; }
    public UI_Upgrades upgradesUI { get; private set; }
    public UI_SelectCharacter selectCharacter { get; private set; }
    #endregion

    #region Tooltips
    public UI_StatTooltip statTooltip { get; private set; }
    public UI_UpgradeStatPointTooltip upgradeStatPointTooltip { get; private set; }
    #endregion

    private bool hasEndedGame;
    private Coroutine openSummaryCo;

    private void Awake()
    {
        instance = this;

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        skillBoardUI = GetComponentInChildren<UI_SkillBoard>(true);
        statsUI = GetComponentInChildren<UI_Stats>(true);
        totalSummaryUI = GetComponentInChildren<UI_TotalSummary>(true);
        settingsUI = GetComponentInChildren<UI_Settings>(true);
        fadeUI = GetComponentInChildren<UI_FadeScreen>(true);
        upgradesUI = GetComponentInChildren<UI_Upgrades>(true);
        statTooltip = GetComponentInChildren<UI_StatTooltip>(true);
        upgradeStatPointTooltip = GetComponentInChildren<UI_UpgradeStatPointTooltip>(true);
        selectCharacter = GetComponentInChildren<UI_SelectCharacter>(true);
    }

    private void Start()
    {
        RegisterAllButtonSounds();
    }

    private void RegisterAllButtonSounds()
    {
        buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(AudioManager.instance.PlayButtonClickSFX);
            button.onClick.AddListener(AudioManager.instance.PlayButtonClickSFX);
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        OnSkillSlotChange += ingameUI.skillHolder.SetupSkillSlots;
        OnPlayerSet?.Invoke();

        ingameUI.skillHolder.SetupSkillSlots();
    }

    private void OnDestroy()
    {
        if (ingameUI != null && ingameUI.skillHolder != null)
            OnSkillSlotChange -= ingameUI.skillHolder.SetupSkillSlots;
    }

    public void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
        {
            Time.timeScale = 0f;
            ControlsManager.instance.inputActions.Player.Disable();
        }
        else
        {
            Time.timeScale = 1f;
            ControlsManager.instance.inputActions.Player.Enable();
        }
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
        statsUI.playerStatsUI.UpdateStatUI();

        AudioManager.instance.PlayGlobalSFX("ui_open");

        StopPlayerControlIfNeeded();
    }

    public void OpenSettings()
    {
        SwitchTo(settingsUI.gameObject);

        AudioManager.instance.PlayGlobalSFX("ui_open");

        StopPlayerControlIfNeeded();
    }

    public void OpenSummary(float duration)
    {
        if (openSummaryCo != null)
            StopCoroutine(openSummaryCo);

        openSummaryCo = StartCoroutine(OpenTotalSummaryCo(duration));
    }

    private IEnumerator OpenTotalSummaryCo(float duration)
    {
        yield return new WaitForSeconds(duration);

        SwitchTo(totalSummaryUI.gameObject);
        totalSummaryUI.UpdateTotalSummary();

        //AudioManager.instance.PlayGlobalSFX("ui_open");

        StopPlayerControlIfNeeded();

        openSummaryCo = null;
    }

    public void SwitchToIngameUI()
    {
        StopPlayerControls(false);
        SwitchTo(ingameUI.gameObject);
    }

    public void EndGame()
    {
        if (hasEndedGame) return;
        hasEndedGame = true;

        SwitchTo(totalSummaryUI.gameObject);
        totalSummaryUI.UpdateTotalSummary();
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.TotalSouls += GameManager.instance.SoulsGained;
        GameManager.instance.SoulsGained = 0;
        GameManager.instance.ChangeScene("MainMenu");
    }

    public void PlayBtn()
    {
        GameManager.instance.ChangeScene("Level 1");
    }


    public void QuitBtn()
    {
        StopAllCoroutines();
        Time.timeScale = 1f; // Reset timeScale nếu game đang pause

        Application.Quit();
    }

    public bool IsInGameUI() => ingameUI.gameObject.activeSelf;

    public bool IsSkillBoardUI() => skillBoardUI.gameObject.activeSelf;

    public void OnSkillSlotChangeInvoke()
    {
        OnSkillSlotChange?.Invoke();
    }
}
