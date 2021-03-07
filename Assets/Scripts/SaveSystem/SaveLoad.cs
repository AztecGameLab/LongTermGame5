using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;

#endif

namespace SaveSystem
{
    public static class SaveLoad
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


        #region Public functions for saving and loading the current scene

        //get the dictionary that contains all scenes and put the current scene into that dictionary and save
        [EasyButtons.Button]
        static public void SaveCurrentScene()
        {
            SaveScene(SceneManager.GetActiveScene().name);
        }

        [EasyButtons.Button]
        static public void SaveScene(string sceneName) //TODO make gather scene save data work for non-main scenes
        {
            var Dict_SceneName_GameObjectIDs = LoadGameFile();
            Dict_SceneName_GameObjectIDs[sceneName] = GatherSceneSaveData();
            SaveGameFile(Dict_SceneName_GameObjectIDs);

            Debug.Log("\"" + sceneName + "\" scene was saved");
        }

        [EasyButtons.Button]
        static public void LoadScene(string sceneName) //TODO make restore scene save data work for non-main scenes
        {
            var Dict_SceneName_GameObjectIDs = LoadGameFile();
            if (Dict_SceneName_GameObjectIDs.TryGetValue(sceneName,
                out Dictionary<string, Dictionary<string, SaveData>> Dict_GameObjectIDs_ComponentTypes))
            {
                RestoreSceneSaveData(Dict_GameObjectIDs_ComponentTypes);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                Debug.Log("\"" + sceneName + "\" save loaded");
#endif
            }
            else
            {
                Debug.LogWarning("\"" + sceneName + "\" save not found");
            }
        }

        //get save file then find the current scene and load that data into the current scene
        [EasyButtons.Button]
        static public void LoadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion


        #region Manage Scene <-> Dictionary translations

        static Dictionary<string, Dictionary<string, SaveData>> GatherSceneSaveData()
        {
            var Dict_GameObjectIDs_ComponentTypes = new Dictionary<string, Dictionary<string, SaveData>>();

            foreach (var saveableGameObject in UnityEngine.Object.FindObjectsOfType<SaveableGameObject>()
            ) //for each saveable GameObject in the current scene
            {
                Dict_GameObjectIDs_ComponentTypes[saveableGameObject.id] =
                    saveableGameObject.GatherComponentsSaveData();
            }

            return Dict_GameObjectIDs_ComponentTypes;
        }

        static void RestoreSceneSaveData(
            Dictionary<string, Dictionary<string, SaveData>> Dict_GameObjectIDs_ComponentTypes)
        {
            foreach (var saveableGameObject in UnityEngine.Object.FindObjectsOfType<SaveableGameObject>()
            ) //TODO needs to find objects of type in a specific async loaded scene
            {
                if (Dict_GameObjectIDs_ComponentTypes.TryGetValue(saveableGameObject.id,
                    out Dictionary<string, SaveData> ComponentTypes_SaveData_Dict))
                {
                    saveableGameObject.RestoreComponentsSaveData(ComponentTypes_SaveData_Dict);
                }
            }
        }

        #endregion


        #region Manages saving/loading actual files

        //save the game data in binary
        static void SaveGameFile(
            Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> Dict_SceneName_GameObjectIDs)
        {
            SaveGameJSON(new SaveDisplay(Dict_SceneName_GameObjectIDs));

            var gameSavePath =
                Application.persistentDataPath +
                String.Format("/{0}.save.AGL", GetDateTimeString()); //create a save file with the current date and time
            using (var stream = File.Open(gameSavePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, Dict_SceneName_GameObjectIDs);
            }
        }

        //load the binary game data
        static Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>> LoadGameFile()
        {
            var path = GetMostRecentFile(".AGL");
            if (path == "")
            {
                return new Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, Dictionary<string, Dictionary<string, SaveData>>>) formatter.Deserialize(
                    stream);
            }
        }

        #endregion


        #region Helper Functions

        static string GetDateTimeString()
        {
            DateTime localDate = DateTime.Now;
            return (
                $"{localDate.Month.ToString("00")}.{localDate.Day.ToString("00")}.{localDate.Year}_{localDate.Hour.ToString("00")}.{localDate.Minute.ToString("00")}.{localDate.Second.ToString("00")}"
            );
        }

        static string GetMostRecentFile(string extension)
        {
            var directory = new DirectoryInfo(Application.persistentDataPath);
            var files = directory.GetFiles("*" + extension);
            if (files.Length == 0)
                return "";
            string file = (files.OrderByDescending(f => f.LastWriteTime).First()).FullName;
            return (file);
        }

        #endregion


        #region Manages easyer to read JSON save file for debugging

        static void SaveGameJSON(SaveDisplay saveDisplay)
        {
            var JsonSavePath = Application.persistentDataPath + String.Format("/{0}.save.JSON", GetDateTimeString());
            string json = JsonUtility.ToJson(saveDisplay, true);
            File.WriteAllText(JsonSavePath, json);
        }

        #endregion
    }
}