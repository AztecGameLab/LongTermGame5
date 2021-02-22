using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    /*
        GameFile is Dictionary<SceneNames, Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>>

        GameFile -> SceneName -> GameObject -> ComponentType -> SaveData -> SavedVariables
    */

    static string gameSavePath;
    static string SettingsSavePath;

    private void Start()
    {
        gameSavePath = Application.persistentDataPath + "/savefile.AGL"; //can make multiple saves by saving with different names
        SettingsSavePath = Application.persistentDataPath + "/settings.AGLs"; //can make multiple saves by saving with different names
    }


    //get the dictionary of all scenes and put the current scene into that dictionary
    [EasyButtons.Button]
    public void SaveCurrentScene()
    {
        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();
        Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData[SceneManager.GetActiveScene().name] = GatherGameObjectsSaveData();
        SaveGameFile(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
    }

    [EasyButtons.Button]
    public void LoadCurrentScene()
    {
        //get file
        //get this scene and restore that

        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();
        RestoreGameObjectsSaveData(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData[SceneManager.GetActiveScene().name]);
    }

    void SaveGameFile(Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData)
    {
        using (var stream = File.Open(gameSavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
        }
    }

    Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> LoadGameFile()
    {
        if (!File.Exists(gameSavePath))
        {
            return new Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>();
        }

        using (FileStream stream = File.Open(gameSavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>)formatter.Deserialize(stream);
        }
    }

    Dictionary<string, Dictionary<string, SaveData>> GatherGameObjectsSaveData()
    {
        var Dict_GameObjectIDs_ComponentTypes_SaveData = new Dictionary<string, Dictionary<string, SaveData>>();

        foreach (var saveableGameObject in FindObjectsOfType<SaveableGameObject>()) //for each saveable GameObject in the current scene
        {
            Dict_GameObjectIDs_ComponentTypes_SaveData[saveableGameObject.id] = saveableGameObject.GatherComponentsSaveData();
        }

        return Dict_GameObjectIDs_ComponentTypes_SaveData;
    }

    void RestoreGameObjectsSaveData(Dictionary<string, Dictionary<string, SaveData>> Dict_GameObjectIDs_ComponentTypes_SaveData)
    {
        foreach (var saveableGameObject in FindObjectsOfType<SaveableGameObject>())
        {
            if (Dict_GameObjectIDs_ComponentTypes_SaveData.TryGetValue(saveableGameObject.id, out Dictionary<string, SaveData> ComponentTypes_SaveData_Dict))
            {
                saveableGameObject.RestoreComponentsSaveData(ComponentTypes_SaveData_Dict);
            }
        }
    }
}