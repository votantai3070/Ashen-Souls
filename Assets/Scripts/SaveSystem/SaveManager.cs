using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private readonly List<ISaveable> allSaveables = new();

    [SerializeField] private string fileName = "save.json";
    [SerializeField] private bool encryption = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshSaveables();
        LoadGame();
    }

    private void RefreshSaveables()
    {
        allSaveables.Clear();

        MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i] is ISaveable saveable)
            {
                allSaveables.Add(saveable);
            }
        }
    }

    public void LoadGame()
    {
        if (dataHandler == null)
        {
            Debug.LogError("DataHandler is NULL in SaveManager.LoadGame()");
            return;
        }

        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save data found, creating new save!");
            gameData = new GameData();
            return;
        }

        for (int i = 0; i < allSaveables.Count; i++)
        {
            allSaveables[i].LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (dataHandler == null)
        {
            Debug.LogError("DataHandler is NULL in SaveManager.SaveGame()");
            return;
        }

        for (int i = 0; i < allSaveables.Count; i++)
        {
            allSaveables[i].SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("**** Delete save data ****")]
    public void DeleteSaveData()
    {
        dataHandler.Delete();
        gameData = new GameData();
        RefreshSaveables();
        LoadGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveGame();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}