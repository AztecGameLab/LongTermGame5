using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class LevelToolkitWindow : EditorWindow
    {
        private static Scene _activeScene;
        private static Dictionary<string, Level> _levels = new Dictionary<string, Level>();
        
        private static bool IsValidLevel => _levels.ContainsKey(_activeScene.name);
        private static bool HasNeighbors => ActiveLevel.levelsToPreload.Length > 0;
        private static Level ActiveLevel => IsValidLevel ? _levels[_activeScene.name] : null;
        
        private void OnInspectorUpdate()
        {
            UpdateLevelDictionary();
        }

        [MenuItem("LTG/Level Design Toolkit")]
        private static void ShowWindow()
        {
            UpdateLevelDictionary();
            
            var window = GetWindow<LevelToolkitWindow>();
            window.titleContent = new GUIContent("Level Design Toolkit");
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
                LevelInformation();
        }

        private static void DisplayHeaderInformation()
        {
            GUILayout.Space(30f);
            GUILayout.Label($"Current Scene: { _activeScene.name } [{ (IsValidLevel ? "VALID" : "INVALID") }]");
            
            GUILayout.BeginHorizontal();
            
            GenerateLevelButton();
            OpenScenesButton();
            LoadPersistentSceneButton();
            
            GUILayout.EndHorizontal();
        }

        private static void LoadPersistentSceneButton()
        {
            if (GUILayout.Button("Persistent Scene"))
            {
                Debug.Log("Loading persistent scene!");
                EditorUtil.EnsureSceneIsLoaded("Persistent");
            }
        }

        private static void LevelInformation()
        {
            PlayerSpawnInformation();
            PreloadInformation();
            LevelTriggerInformation();
        }

        private static void PlayerSpawnInformation()
        {
            GUILayout.Space(15f);

            if (EditorUtil.HasPlayerSpawn(out var playerSpawn))
            {
                GUILayout.Label($"Player Spawn: { playerSpawn.transform.position }");
            }
            else
            {
                PlayerSpawnButton();
            }
        }

        private static void PlayerSpawnButton()
        {
            GUILayout.Label("Player Spawn: None");
                
            if (GUILayout.Button("Generate Player Spawn"))
            {
                var gameObject = new GameObject { tag = "PlayerSpawn", name = "Player Spawn"};
                Selection.activeGameObject = gameObject;
                Undo.RegisterCreatedObjectUndo(gameObject, "Generate Player Spawn");
            }
        }
        
        private static void LevelTriggerInformation()
        {
            GUILayout.Space(15f);

            if (GUILayout.Button("Generate Level Trigger"))
                GenerateLevelTrigger();
        }

        private static void GenerateLevelTrigger()
        {
            LevelLoadTrigger levelTrigger;
            
            if (!Scanner.HasObjectsInScene<LevelLoadTrigger>(out var results))
            {
                var temp = new GameObject("Level Trigger");
                temp.AddComponent<PolygonCollider2D>();
                temp.AddComponent<LevelLoadTrigger>();

                levelTrigger = temp.GetComponent<LevelLoadTrigger>();
                Undo.RegisterCreatedObjectUndo(temp, "Generate Level Trigger");
            }
            else
            {
                levelTrigger = results[0];
            }
            
            levelTrigger.GenerateCollider();
            levelTrigger.currentLevel = ActiveLevel;
            Selection.activeGameObject = levelTrigger.gameObject;
            EditorUtility.FocusProjectWindow();
            EditorSceneManager.MarkSceneDirty(_activeScene);
        }

        private static void PreloadInformation()
        {
            GUILayout.Space(15f);

            GUILayout.Label("Neighbors: " + (HasNeighbors ? ActiveLevel.levelsToPreload.Length.ToString() : "None"));

            if (HasNeighbors)
            {
                foreach (var neighbor in ActiveLevel.levelsToPreload)
                    NeighborInformation(neighbor);
            }
        }

        private static void NeighborInformation(Level neighbor)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(neighbor.sceneName);

            if (GUILayout.Button("Load"))
                EditorUtil.EnsureSceneIsLoaded(neighbor.sceneName);
                    
            if (GUILayout.Button("Unload"))
                EditorUtil.EnsureSceneIsUnloaded(neighbor.sceneName);

            GUILayout.EndHorizontal();
        }

        private static void GenerateLevelButton()
        {
            if (IsValidLevel)
            {
                if (GUILayout.Button("Open Level In Inspector"))
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

        private static void OpenScenesButton()
        {
            if (GUILayout.Button("Scenes Folder"))
            {
                EditorUtility.FocusProjectWindow();
                var results = AssetDatabase.FindAssets("t:Object", new [] { "Assets/Scenes"});
                var scenePath = AssetDatabase.GUIDToAssetPath(results[0]);
                var scene = AssetDatabase.LoadAssetAtPath<Object>(scenePath);

                Selection.activeObject = scene;
            }
        }
    }
}