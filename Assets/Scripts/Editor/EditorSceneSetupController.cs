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
            playerSpawn = null;
            Scene scene = SceneManager.GetActiveScene();
            var gameObjects = scene.GetRootGameObjects();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject.CompareTag("PlayerSpawn"))
                    playerSpawn = gameObject;
                
                foreach (var gameObjectChild in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (gameObjectChild.CompareTag("PlayerSpawn"))
                        playerSpawn = gameObjectChild.gameObject;
                }
            }

            return playerSpawn != null;
        }

        private static void CreatePlayerAt(Vector3 position)
        {
            var player = Resources.Load<Transform>("TempPlayer");
            player = Object.Instantiate(player);
            player.position = position;
        }
    }
}