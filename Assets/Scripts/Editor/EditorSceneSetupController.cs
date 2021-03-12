using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class EditorSceneSetupController
    {
        [RuntimeInitializeOnLoadMethod]
        private static void LoadDependentScenes()
        {
            EnsureSceneIsLoaded("Persistent");

            if (HasPlayerSpawn(out var playerSpawn))
                CreatePlayerAt(playerSpawn.transform.position);
        }

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
            playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
            return playerSpawn != null;
        }

        private static void CreatePlayerAt(Vector3 position)
        {
            var player = Resources.Load<Transform>("TempPlayer");
            Object.Instantiate(player);
            player.position = position;
        }
    }
}