using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static bool GetSaveFiles()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!File.Exists(GetPath(i)))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string path = GetPath(i);
                FileStream stream = new FileStream(path, FileMode.Create);

                PlayerData data = new PlayerData(new W5ScoreManager(), i);
                formatter.Serialize(stream, data);
                stream.Close();
            }
        }

        return true;
    }

    public static void SaveLevel(W5ScoreManager scoreManager, int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetPath(level);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(scoreManager, level);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveStats()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + W8SaveData.savePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(W8SaveData.w8SaveData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveStats(W8SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + W8SaveData.savePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(saveData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    static string GetPath(int i)
    {
        switch (i)
        {
            case 0:
                return Application.persistentDataPath + "/week5Level0.txt";
                break;
            case 1:
                return Application.persistentDataPath + "/week5Level1.txt";
                break;
            case 2:
                return Application.persistentDataPath + "/week5Level2.txt";
                break;
            case 3:
                return Application.persistentDataPath + "/week5Level3.txt";
                break;
            case 4:
                return Application.persistentDataPath + "/week5Level4.txt";
                break;
            default:
                return Application.persistentDataPath + "/week5Level0.txt";

        }
    }

    public static PlayerData LoadLevel(int i)
    {
        string path = GetPath(i);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string newPath = GetPath(i);
            FileStream stream = new FileStream(newPath, FileMode.Create);

            PlayerData data = new PlayerData(new W5ScoreManager(), i);
            formatter.Serialize(stream, data);
            stream.Close();

            //Debug.LogError("Save file not found in " + path);
            return data;
        }
    }

    public static PlayerData LoadLevel(string s)
    {
        string path = Application.persistentDataPath + s;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        PlayerData data = formatter.Deserialize(stream) as PlayerData;
        stream.Close();

        return data;
    }
}
