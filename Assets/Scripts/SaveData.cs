using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int currentLevel;
}

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    public GameData data;

    private string path;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        path = Application.persistentDataPath + "/data.json";
        if (!File.Exists(path))
        {
            return;
        }
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to load data: {e}");
        }
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/data.json";
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(JsonUtility.ToJson(data));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data: {e}");
        }
    }
}
