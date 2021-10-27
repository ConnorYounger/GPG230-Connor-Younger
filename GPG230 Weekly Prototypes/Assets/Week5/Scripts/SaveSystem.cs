using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevel(W5ScoreManager scoreManager, int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetPath(level);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(scoreManager, level);

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
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
