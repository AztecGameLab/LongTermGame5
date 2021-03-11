using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using EasyButtons;
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
        GameObjectData is "Dictionary<ComponentTypeNames, SaveData>"
        SaveData contains saved information about a component

        Game Data is Dictionary<SceneNames, Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>>
        Scene Data is                       Dictionary<GameObjectIDs, Dictionary<ComponentTypes, SaveData>>
        GameObject Data is                                            Dictionary<ComponentTypes, SaveData>
        Component Data is stored in                                                              SaveData
        */


        private static GameData tempCurrentGame = new GameData(); //temporary data of the current game that isnt saved to a file yet


        #region only game system call these, not the player //TODO make these work for multiple loaded scenes. I think right now there no difference between SaveScene and SaveActiveScene because FindObjectsOfType searches all loaded scenes

        public static void SetPlayerCurrentScene(string sceneName)
        {
            tempCurrentGame.playerCurrentScene = sceneName;
        }

        public static string GetPlayerCurrentScene()
        {
            return tempCurrentGame.playerCurrentScene;
        }

        [Button]
        public static void SaveSceneToTempData(string sceneName)
        {
            tempCurrentGame.dict[sceneName] = GatherSceneSaveData();
            Debug.Log("\"" + sceneName + "\" scene was saved to tempCurrentGame");
        }

        [Button]
        public static void SaveActiveSceneToTempData()
        {
            SaveSceneToTempData(SceneManager.GetActiveScene().name);
        }

        [Button]
        public static void LoadSceneFromTempData(string sceneName)
        {
            if (tempCurrentGame.dict.TryGetValue(sceneName, out SceneData sceneData))
            {
                RestoreSceneSaveData(sceneData);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                Debug.Log("\"" + sceneName + "\" save loaded");
#endif
            }
            else
            {
                Debug.LogWarning("\"" + sceneName + "\" save not found in tempCurrentGame");
            }
        }

        [Button]
        public static void LoadActiveSceneFromTempData()
        {
            LoadSceneFromTempData(SceneManager.GetActiveScene().name);
        }

        #endregion


        [Button]
        public static void SaveTempDataToFile() //call this when the player saves //TODO make it work for multiple loaded scenes
        {
            SaveGameFile(tempCurrentGame);
            Debug.Log("tempCurrentGame was saved to file");
        }

        [Button]
        public static void LoadFromFileToTempData() //call this when the player loads a game //TODO make it work for multiple loaded scenes
        {
            tempCurrentGame = LoadMostRecentGameFile();
        }


//         #region old functions for saving and loading a scene
//         
//         //get the dictionary that contains all scenes and put the scene into that dictionary and save
//         [EasyButtons.Button]
//         public static void SaveSceneDirectlyToFile(string sceneName) 
//         {
//             GameData gameData = LoadMostRecentGameFile();
//             gameData.dict[sceneName] = GatherSceneSaveData();
//             SaveGameFile(gameData);
//
//             Debug.Log("\"" + sceneName + "\" scene was saved");
//         }
//
//         [EasyButtons.Button]
//         public static void SaveCurrentSceneDirectlyToFile()
//         {
//             SaveSceneDirectlyToFile(SceneManager.GetActiveScene().name);
//         }
//
//
//         //get save file then find the scene and load that data into the current scenes
//         [EasyButtons.Button]
//         public static void LoadSceneDirectlyFromFile(string sceneName)
//         {
//             GameData gameData = LoadMostRecentGameFile();
//             if (gameData.dict.TryGetValue(sceneName, out SceneData sceneData))
//             {
//                 RestoreSceneSaveData(sceneData);
// #if UNITY_EDITOR
//                 if (!Application.isPlaying)
//                     EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
//                 Debug.Log("\"" + sceneName + "\" save loaded");
// #endif
//             }
//             else
//             {
//                 Debug.LogWarning("\"" + sceneName + "\" save not found");
//             }
//         }
//
//         [EasyButtons.Button]
//         public static void LoadCurrentSceneDirectlyFromFile()
//         {
//             LoadSceneDirectlyFromFile(SceneManager.GetActiveScene().name);
//         }
//
//         #endregion


        #region Current Scenes <-> Dictionary

        static SceneData GatherSceneSaveData()
        {
            var sceneData = new SceneData();

            foreach (var saveableGameObject in UnityEngine.Object.FindObjectsOfType<SaveableGameObject>()
            ) //for each saveable GameObject in the current scene
            {
                sceneData.dict[saveableGameObject.id] = saveableGameObject.GatherComponentsSaveData();
            }

            return sceneData;
        }

        static void RestoreSceneSaveData(
            SceneData sceneData)
        {
            foreach (var saveableGameObject in UnityEngine.Object.FindObjectsOfType<SaveableGameObject>()
            ) //TODO needs to find objects of type in a specific async loaded scene
            {
                if (sceneData.dict.TryGetValue(saveableGameObject.id, out GameObjectData gameObjectData))
                {
                    saveableGameObject.RestoreComponentsSaveData(gameObjectData);
                }
            }
        }

        #endregion


        #region Manages saving/loading actual files

        //save the game data in binary
        static void SaveGameFile(GameData gameData)
        {
            SaveGameJSON(new SaveDisplay(gameData));

            var gameSavePath =
                Application.persistentDataPath +
                String.Format("/{0}_save.AGL", GetDateTimeString()); //create a save file with the current date and time
            using (var stream = File.Open(gameSavePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, gameData);
            }
        }

        //load the binary game data
        static GameData LoadMostRecentGameFile()
        {
            var path = GetMostRecentFile(".AGL");
            if (path == "")
            {
                return new GameData();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (GameData) formatter.Deserialize(stream);
            }
        }

        #endregion


        #region Helper Functions

        static string GetDateTimeString()
        {
            DateTime localDate = DateTime.Now;
            return (
                $"{localDate.Month.ToString("00")}-{localDate.Day.ToString("00")}-{localDate.Year}_{localDate.Hour.ToString("00")}.{localDate.Minute.ToString("00")}.{localDate.Second.ToString("00")}"
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


        #region classes for debugger JSON SaveDisplay

        [System.Serializable]
        class SaveDisplay
        {
            public string playerCurrentScene;
            public List<SceneSaveDisplay> scenes = new List<SceneSaveDisplay>();
            public SaveDisplay(GameData gameData)
            {
                playerCurrentScene = gameData.playerCurrentScene;
                foreach (KeyValuePair<string, SceneData> item in gameData.dict)
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

            public SceneSaveDisplay(string key, SceneData save)
            {
                sceneName = key;
                foreach (KeyValuePair<string, GameObjectData> item in save.dict)
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

            public GameObjectSaveDisplay(string key, GameObjectData save)
            {
                gameobjectID = key;
                foreach (KeyValuePair<string, ISaveData> item in save.dict)
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

        #region Manages easyer to read JSON save file for debugging

        static void SaveGameJSON(SaveDisplay saveDisplay)
        {
            var JsonSavePath = Application.persistentDataPath +
                               String.Format("/{0}_DEBUG_save.JSON", GetDateTimeString());
            string json = JsonUtility.ToJson(saveDisplay, true);
            File.WriteAllText(JsonSavePath, json);
        }

        #endregion
    }
}