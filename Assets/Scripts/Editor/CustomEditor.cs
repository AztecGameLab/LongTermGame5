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
            
            if (!IsValidLevel)
                DisplayInvalidLevelScreen();
        }

        private static void DisplayHeaderInformation()
        {
            GUILayout.Space(30f);
            GUILayout.Label($"Current Scene: { _activeScene.name } [{ (IsValidLevel ? "VALID" : "NO LEVEL") }]");
        }
        
        private static void DisplayValidLevelScreen()
        {
            //     - provide option to load neighbors
            //     - if the level doesn't have a playerStart, add option to auto-generate one
            //     - if the level doesn't have a level trigger yet, add option to auto-generate one
            
            if (EditorSceneSetupController.HasPlayerSpawn(out var playerSpawn))
            {
                GUILayout.Label($"Player spawn found at { playerSpawn.transform.position }");
            }
            else
            {
                if (GUILayout.Button("Generate PlayerSpawn"))
                {
                    Debug.Log("Generate playerspawn!");
                }
            }

            if (ActiveLevel.levelsToPreload.Length > 0)
            {
                GUILayout.Label("Levels to preload: ");

                foreach (var level in ActiveLevel.levelsToPreload)
                {
                    GUILayout.Label(level.sceneName);
                }
            }
        }

        private static void DisplayInvalidLevelScreen()
        {
            GenerateLevelButton();
        }

        private static void GenerateLevelButton()
        {
            if (GUILayout.Button("Generate Level"))
            {
                var level = CreateInstance<Level>();
                level.sceneName = _activeScene.name;
                
                AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{ level.sceneName }.asset");
                AssetDatabase.SaveAssets();
                
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = level;
            }
        }
    }
}