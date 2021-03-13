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
        
        public static bool HasPlayerSpawn(out GameObject playerSpawn)
        {
            if (Scanner.HasObjectsInScene(out Transform[] playerSpawnTransform, o => o.CompareTag("PlayerSpawn")))
            {
                playerSpawn = playerSpawnTransform[0].gameObject;
                return true;
            }

            playerSpawn = null;
            return false;
        }
    }
}