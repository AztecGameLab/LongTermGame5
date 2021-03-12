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
            if (!SceneManager.GetSceneByName(sceneName).IsValid())
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
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