using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    static string gameSavePath;
    static string SettingsSavePath;

    private void Start()
    {
        gameSavePath = Application.persistentDataPath + "/savefile.AGL"; //can make multiple saves by saving with different names
        SettingsSavePath = Application.persistentDataPath + "/settings.AGLs"; //can make multiple saves by saving with different names
    }

    [EasyButtons.Button]
    public void Save()
    {
        var state = LoadFile();
        CaptureAllStates(state);
        SaveFile(state);
    }

    [EasyButtons.Button]
    public void Load()
    {
        var state = LoadFile();
        RestoreAllStates(state);
    }

    void CaptureAllStates(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.id] = saveable.CaptureComponentStates();
        }
    }

    void RestoreAllStates(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.id, out object value))
            {
                saveable.RestoreComponentStates(value);
            }
        }
    }


    void SaveFile(object state)
    {
        using (var stream = File.Open(gameSavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(gameSavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(gameSavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }



}