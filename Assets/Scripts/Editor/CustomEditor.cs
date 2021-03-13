using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class CustomEditor : EditorWindow
    {
        private static Scene _activeScene;
        private static Dictionary<string, Level> _levels = new Dictionary<string, Level>();
        
        private static bool IsValidLevel => _levels.ContainsKey(_activeScene.name);
        private static Level ActiveLevel => IsValidLevel ? _levels[_activeScene.name] : null;
        
        private void OnInspectorUpdate()
        {
            UpdateLevelDictionary();
        }

        [MenuItem("LTG/Level Generator")]
        private static void ShowWindow()
        {
            UpdateLevelDictionary();
            
            var window = GetWindow<CustomEditor>();
            window.titleContent = new GUIContent("Level Generator");
            window.Show();
        }
        
        private static void UpdateLevelDictionary()
        {
            var levels = Resources.LoadAll<Level>("Levels");
            _levels = levels.ToDictionary(level => level.sceneName);
        }

        private void OnGUI()
        {
            _activeScene = SceneManager.GetActiveScene();
            
            DisplayHeaderInformation();
            
            if (IsValidLevel)
                DisplayValidLevelScreen();
        }

        private static void DisplayHeaderInformation()
        {
            GUILayout.Space(30f);
            GUILayout.Label($"Current Scene: { _activeScene.name } [{ (IsValidLevel ? "VALID" : "INVALID") }]");
            
            GenerateLevelButton();
        }
        
        private static void DisplayValidLevelScreen()
        {
            GeneratePlayerSpawnInformation();
            GeneratePreloadInformation();
            GenerateLevelTriggerInformation();
        }

        private static void GenerateLevelTriggerInformation()
        {
            GUILayout.Space(30f);
            
            if (GUILayout.Button("Generate Level Trigger"))
            {
                var gameObjects = _activeScene.GetRootGameObjects();
                LevelLoadTrigger levelTrigger = null;
                
                foreach (var rootObj in gameObjects)
                {
                    levelTrigger = rootObj.GetComponentInChildren<LevelLoadTrigger>();
                    
                    if (levelTrigger != null)
                        break;
                }

                if (levelTrigger == null)
                {
                    var newLevelTrigger = new GameObject("Level Trigger");
                    newLevelTrigger.AddComponent<PolygonCollider2D>();
                    levelTrigger = newLevelTrigger.AddComponent<LevelLoadTrigger>();
                }
                
                levelTrigger.GenerateCollider();
                levelTrigger.currentLevel = ActiveLevel;

                Selection.activeGameObject = levelTrigger.gameObject;
                Undo.RegisterCreatedObjectUndo(levelTrigger, "Generate Player Spawn");
                EditorUtility.FocusProjectWindow();
            }
        }

        private static void GeneratePreloadInformation()
        {
            GUILayout.Space(15f);
            
            if (ActiveLevel.levelsToPreload.Length > 0)
            {
                GUILayout.Label("Levels to preload: ");

                foreach (var level in ActiveLevel.levelsToPreload)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(level.sceneName);

                    if (GUILayout.Button("Load"))
                    {
                        EditorSceneSetupController.EnsureSceneIsLoaded(level.sceneName);
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.sceneName));
                    }
                    
                    if (GUILayout.Button("Unload"))
                        EditorSceneSetupController.EnsureSceneIsUnloaded(level.sceneName);

                    GUILayout.EndHorizontal();
                }
            }
        }
        
        private static void GeneratePlayerSpawnInformation()
        {
            GUILayout.Space(15f);

            if (EditorSceneSetupController.HasPlayerSpawn(out var playerSpawn))
            {
                GUILayout.Label($"Player Spawn: { playerSpawn.transform.position }");
            }
            else
            {
                GUILayout.Label("Player Spawn: None");
                
                if (GUILayout.Button("Generate Player Spawn"))
                {
                    var gameObject = new GameObject { tag = "PlayerSpawn", name = "Player Spawn"};
                    Selection.activeGameObject = gameObject;
                    Undo.RegisterCreatedObjectUndo(gameObject, "Generate Player Spawn");
                }
            }
        }

        private static void GenerateLevelButton()
        {
            if (IsValidLevel)
            {
                if (GUILayout.Button("View Level In Inspector"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = ActiveLevel;
                }
            }
            else
            {
                if (GUILayout.Button("Generate Level"))
                {
                    var level = CreateInstance<Level>();
                    level.sceneName = _activeScene.name;
                
                    AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{ level.sceneName }.asset");
                    AssetDatabase.SaveAssets();
                
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = level;
                    
                    Undo.RegisterCreatedObjectUndo(level, "Generate Level " + level.sceneName);
                }
            }
        }
    }
}