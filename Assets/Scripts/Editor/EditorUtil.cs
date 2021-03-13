using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class EditorUtil
    {
        public static void EnsureSceneIsLoaded(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
                return;

            if (Application.isPlaying)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                var results = AssetDatabase.FindAssets(sceneName, new [] { "Assets/Scenes" });
                var scenePath = AssetDatabase.GUIDToAssetPath(results[0]);
                
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }
        
        public static void EnsureSceneIsUnloaded(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                EditorSceneManager.CloseScene(scene, true);
            }
        }
        
        public static bool FindObjectInScene<T>(string sceneName, out T result, Predicate<GameObject> shouldSelect = null) 
            where T : Component
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            var rootGameObjects = scene.GetRootGameObjects();
            result = null;
            
            foreach (var rootObject in rootGameObjects)
            {
                foreach (var gameObjectChild in rootObject.GetComponentsInChildren<T>())
                {
                    if (shouldSelect != null && shouldSelect(gameObjectChild.gameObject))
                        result = gameObjectChild;
                }
            }
            
            return result != null;
        }

        public static bool FindObjectInScene<T>(out T result, Predicate<GameObject> shouldSelect = null)
            where T : Component
        {
            var sceneName = SceneManager.GetActiveScene().name;
            return FindObjectInScene(sceneName, out result, shouldSelect);
        }
        
        public static bool TryGetPlayerSpawn(out GameObject playerSpawn)
        {
            var result = FindObjectInScene(out Transform playerSpawnTransform, o => o.CompareTag("PlayerSpawn"));
            playerSpawn = playerSpawnTransform.gameObject;
            return result;
        }
    }
}