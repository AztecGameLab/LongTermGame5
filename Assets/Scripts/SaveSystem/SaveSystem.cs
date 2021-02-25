using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

[ExecuteInEditMode]
public class SaveSystem : MonoBehaviour
{
    /*
        A bunch of attempts to explain how game save data is stored. lmk which makes the most sense -Kain

        Game     -> Scenes     -> GameObjects   -> Components     -> SaveData -> SavedVariables
        savefile -> SceneNames -> GameObjectIDs -> ComponentTypes -> SaveData -> SavedVariables

        GameData is "Dictionary<SceneNames, SceneData>"
        SceneData is "Dictionary<GameObjectIDs, GameObjectData>"
        GameObjectData is "Dictionary<ComponentTypes, SaveData>"
        SaveData contains saved information about a component

        Game Data is Dictionary<SceneNames, Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>>
        Scene Data is                       Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>
        GameObject Data is                                            Dictionary<ComponentTypes, SaveData>
        Component Data is stored in                                                              SaveData
    */


    [SerializeField]
    [ReadOnly]
    SaveDisplay saveDisplay; //json compatable class for debugging
    #region classes for SaveDisplay
    [System.Serializable]
    class SaveDisplay
    {
        public List<SceneSaveDisplay> scenes = new List<SceneSaveDisplay>();
        public SaveDisplay(Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> save)
        {
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, SaveData>>> item in save)
            {
                scenes.Add(new SceneSaveDisplay(item.Key, item.Value));
            }
        }
    }
    [Serializable]
    class SceneSaveDisplay
    {
        public string sceneName;
        public List<GameObjectSaveDisplay> gameobjects = new List<GameObjectSaveDisplay>();
        public SceneSaveDisplay(string key, Dictionary<string, Dictionary<string, SaveData>> save)
        {
            sceneName = key;
            foreach (KeyValuePair<string, Dictionary<string, SaveData>> item in save)
            {
                gameobjects.Add(new GameObjectSaveDisplay(item.Key, item.Value));
            }
        }
    }
    [Serializable]
    class GameObjectSaveDisplay
    {
        public string gameobjectID;
        public List<ComponentSaveDisplay> components = new List<ComponentSaveDisplay>();
        public GameObjectSaveDisplay(string key, Dictionary<string, SaveData> save)
        {
            gameobjectID = key;
            foreach (KeyValuePair<string, SaveData> item in save)
            {
                components.Add(new ComponentSaveDisplay(item.Key, item.Value.ToString()));
            }
        }
    }
    [Serializable]
    class ComponentSaveDisplay
    {
        public string componentType;
        public string saveData;
        public ComponentSaveDisplay(string key, string data)
        {
            componentType = key;
            saveData = data;
        }
    }
    #endregion

    #region singleton
    private static SaveSystem _instance;
    public static SaveSystem instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SaveSystem>();
            }
            return _instance;
        }
    }
    #endregion

    #region save paths
    // static string gameSavePath;
    // static string SettingsSavePath;
    // static string JsonSavePath;

    //     private void OnEnable()
    //     {
    // #if UNITY_EDITOR
    //         //can make multiple saves by saving with different names
    //         gameSavePath = Application.persistentDataPath + String.Format("/{0}.save.EDITOR.AGL", GetDateTimeString());
    //         SettingsSavePath = Application.persistentDataPath + "/settings.EDITOR.AGLs";
    //         JsonSavePath = Application.persistentDataPath + String.Format("/{0}.save.EDITOR.json", GetDateTimeString());
    // #else
    //         //can make multiple saves by saving with different names
    //         gameSavePath = Application.persistentDataPath + String.Format("/{0}.save.AGL", GetDateTimeString());
    //         SettingsSavePath = Application.persistentDataPath + "/settings.AGLs";
    //         JsonSavePath = Application.persistentDataPath + String.Format("/{0}.save.DEV.json", GetDateTimeString());
    // #endif
    //     }
    #endregion


    //get the dictionary that contains all scenes and put the current scene into that dictionary
    [EasyButtons.Button]
    public void SaveCurrentScene()
    {
        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();
        Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData[SceneManager.GetActiveScene().name] = GatherGameObjectsSaveData();
        SaveGameFile(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);

        Debug.Log("\"" + SceneManager.GetActiveScene().name + "\" scene was saved");
    }

    //get scene's save file for current scene and load that data into the current scene
    [EasyButtons.Button]
    public void LoadCurrentScene()
    {
        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();
        if (Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData.TryGetValue(SceneManager.GetActiveScene().name, out Dictionary<string, Dictionary<string, SaveData>> Dict_GameObjectIDs_ComponentTypes_SaveData))
        {
            RestoreGameObjectsSaveData(Dict_GameObjectIDs_ComponentTypes_SaveData);
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("\"" + SceneManager.GetActiveScene().name + "\" save loaded");
        }
        else
        {
            Debug.LogWarning("\"" + SceneManager.GetActiveScene().name + "\" save not found");
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




    //save the game data in binary
    void SaveGameFile(Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData)
    {
        saveDisplay = new SaveDisplay(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
        SaveGameJSON(saveDisplay);

        var gameSavePath = Application.persistentDataPath + String.Format("/{0}.save.AGL", GetDateTimeString()); //create a save file with the current date and time
        using (var stream = File.Open(gameSavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
        }
    }

    //load the binary game data
    Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> LoadGameFile()
    {
        var path = GetMostRecentFile(".AGL");
        if (path != "")
        {
            print("json: " + GetMostRecentFile(".JSON"));
            saveDisplay = LoadGameJSON(GetMostRecentFile(".JSON"));
        }
        else
        {
            return new Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>();
        }

        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>)formatter.Deserialize(stream);
        }
    }

    string GetDateTimeString()
    {
        DateTime localDate = DateTime.Now;
        return ($"{localDate.Month.ToString("00")}.{localDate.Day.ToString("00")}.{localDate.Year}_{localDate.Hour.ToString("00")}.{localDate.Minute.ToString("00")}.{localDate.Second.ToString("00")}");
    }

    string GetMostRecentFile(string extension)
    {
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles("*" + extension);
        print(files.Length);
        if(files.Length == 0)
            return "";
        string file = (files.OrderByDescending(f => f.LastWriteTime).First()).FullName;
        print("FILE: " + file);
        return (file);
    }


    //creates easyer to read JSON sale file debugging
    void SaveGameJSON(SaveDisplay saveDisplay)
    {
        var JsonSavePath = Application.persistentDataPath + String.Format("/{0}.save.JSON", GetDateTimeString());
        string json = JsonUtility.ToJson(saveDisplay, true);
        File.WriteAllText(JsonSavePath, json);
    }
    SaveDisplay LoadGameJSON(string path)
    {
        if (!Directory.Exists(path))
            return null;
        return JsonUtility.FromJson<SaveDisplay>(path);
    }
}