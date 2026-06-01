using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

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

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        //dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryption);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
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
            Debug.Log("No save upgradeData found, creating new save!");
            gameData = new GameData();
            return;
        }

        allSaveables = FindISaveables();

        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    public void SaveGame()
    {
        if (dataHandler == null)
        {
            Debug.LogError("DataHandler is NULL in SaveManager.SaveGame()");
            return;
        }

        allSaveables = FindISaveables();

        //gameData.upgradePoints.Clear();

        foreach (ISaveable saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("**** Delete save upgradeData ****")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryption);
        dataHandler.Delete();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<ISaveable> FindISaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}
