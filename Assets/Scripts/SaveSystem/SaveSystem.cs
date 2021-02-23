using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    /*
        GameFile is Dictionary<SceneNames, Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>>

        GameFile -> SceneName -> GameObject -> ComponentType -> SaveData -> SavedVariables
    */

    #region json compatable classes
    public SaveDisplay saveDisplay;
    [System.Serializable]
    public class SaveDisplay
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
    [System.Serializable]
    public class SceneSaveDisplay
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
    [System.Serializable]
    public class GameObjectSaveDisplay
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
    [System.Serializable]
    public class ComponentSaveDisplay
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

    static string gameSavePath;
    static string SettingsSavePath;

    private void Start()
    {
#if UNITY_EDITOR
        gameSavePath = Application.persistentDataPath + "/savefile.EDITOR.AGL"; //can make multiple saves by saving with different names
        SettingsSavePath = Application.persistentDataPath + "/settings.EDITOR.AGLs"; //can make multiple saves by saving with different names
#else
        gameSavePath = Application.persistentDataPath + "/savefile.AGL"; //can make multiple saves by saving with different names
        SettingsSavePath = Application.persistentDataPath + "/settings.AGLs"; //can make multiple saves by saving with different names
#endif

    }


    //get the dictionary of all scenes and put the current scene into that dictionary
    [EasyButtons.Button]
    public void SaveCurrentScene()
    {
        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();
        Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData[SceneManager.GetActiveScene().name] = GatherGameObjectsSaveData();
        SaveGameFile(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
        Debug.Log("\"" + SceneManager.GetActiveScene().name + "\" scene was saved");
    }

    [EasyButtons.Button]
    public void LoadCurrentScene()
    {
        var Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData = LoadGameFile();

        if (Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData.TryGetValue(SceneManager.GetActiveScene().name, out Dictionary<string, Dictionary<string, SaveData>> Dict_GameObjectIDs_ComponentTypes_SaveData))
        {
            RestoreGameObjectsSaveData(Dict_GameObjectIDs_ComponentTypes_SaveData);
            if(!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("\"" + SceneManager.GetActiveScene().name + "\" save loaded");
        }
        else
        {
            Debug.LogWarning("\"" + SceneManager.GetActiveScene().name + "\" save not found");
        }
    }

    void SaveGameFile(Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData)
    {
        saveDisplay = new SaveDisplay(Dict_SceneName_GameObjectIDs_ComponentTypes_SaveData);
        SaveGameJSON(saveDisplay);

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

    void SaveGameJSON(SaveDisplay saveDisplay)
    {
        var path = Application.persistentDataPath + "/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string json = JsonUtility.ToJson(saveDisplay, true);
#if UNITY_EDITOR
        File.WriteAllText(path + "savefile.EDITOR.JSON", json);
#else
        File.WriteAllText(path + "savefile.JSON", json);
#endif
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