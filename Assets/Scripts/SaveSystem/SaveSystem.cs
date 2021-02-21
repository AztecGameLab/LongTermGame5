using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string gameSavePath = Application.persistentDataPath + "/savefile.AGL"; //can make multiple saves by saving with different names
    static string SettingsSavePath = Application.persistentDataPath + "/settings.AGLs"; //can make multiple saves by saving with different names

    public static void SaveGame(SaveGame saveGame)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(gameSavePath, FileMode.Create);

        formatter.Serialize(stream, saveGame);
        stream.Close();
    }

    public static SaveGame LoadGame()
    {
        if (File.Exists(gameSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(gameSavePath, FileMode.Open);

            SaveGame saveGame = formatter.Deserialize(stream) as SaveGame;
            stream.Close();

            return saveGame;
        }
        else
        {
            Debug.LogError("Game save file not found in:" + gameSavePath);
            return null;
        }
    }

    public static void SaveSettings(SaveSettings saveSettings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SettingsSavePath, FileMode.Create);

        formatter.Serialize(stream, saveSettings);
        stream.Close();
    }

    public static SaveSettings LoadSettings()
    {
        if (File.Exists(SettingsSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SettingsSavePath, FileMode.Open);

            SaveSettings saveSettings = formatter.Deserialize(stream) as SaveSettings;
            stream.Close();

            return saveSettings;
        }
        else
        {
            Debug.LogError("Settings save file not found in:" + SettingsSavePath);
            return null;
        }
    }



}