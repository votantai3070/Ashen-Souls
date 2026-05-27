using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    private bool dataLoaded;

    public int SoulsGained { get; set; } = 0;
    public int TotalSouls { get; set; } = 0;
    public float TotalDamageDealt { get; set; } = 0;
    public float TotalEnemiesKilled { get; set; } = 0;

    private Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        SaveManager.instance.SaveGame();
        StartCoroutine(ChangeSceneCo(sceneName));
    }

    private IEnumerator ChangeSceneCo(string sceneName)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.FadeOut(); // transparent -> black

        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        dataLoaded = false; // data loaded becomes true when you load game from save manager
        yield return null;

        while (dataLoaded == false)
            yield return null;

        fadeScreen = FindFadeScreenUI();
        fadeScreen.FadeIn(); // black -> transparent

        if (player == null)
            yield break;
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
            return UI.instance.fadeUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void LoadData(GameData data)
    {
        TotalSouls = data.souls;

        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        data.souls = SoulsGained;
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "MainMenu")
            return;

        dataLoaded = false;
    }
}
