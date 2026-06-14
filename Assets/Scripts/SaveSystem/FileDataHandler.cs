using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "dorlaz.com";
    private const string WebGLSaveKey = "ASHEN_SOULS_SAVE";

    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encryptData = encryptData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if (encryptData)
                dataToSave = EncryptDecrypt(dataToSave);

#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(WebGLSaveKey, dataToSave);
            PlayerPrefs.Save();
#else
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToSave);
            }
#endif

            Debug.Log("SAVE SUCCESS");
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving data: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadedData = null;

        try
        {
            string dataToLoad = null;

#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerPrefs.HasKey(WebGLSaveKey))
                dataToLoad = PlayerPrefs.GetString(WebGLSaveKey);
#else
            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
#endif

            if (!string.IsNullOrEmpty(dataToLoad))
            {
                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while loading data: " + fullPath + "\n" + e);
        }

        return loadedData;
    }

    public void Delete()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerPrefs.HasKey(WebGLSaveKey))
        {
            PlayerPrefs.DeleteKey(WebGLSaveKey);
            PlayerPrefs.Save();
        }
#else
        if (File.Exists(fullPath))
            File.Delete(fullPath);
#endif
    }

    private string EncryptDecrypt(string data)
    {
        char[] result = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
            result[i] = (char)(data[i] ^ codeWord[i % codeWord.Length]);

        return new string(result);
    }
}