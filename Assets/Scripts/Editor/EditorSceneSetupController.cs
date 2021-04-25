using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Editor.EditorUtil;

namespace Editor
{
    public static class EditorSceneSetupController
    {
        [RuntimeInitializeOnLoadMethod]
        private static async void LoadDependentScenes()
        {
            EnsureSceneIsLoaded("Persistent");

            if (HasPlayerSpawn(out var playerSpawn))
                CreatePlayerAt(playerSpawn.transform.position);
            
            await Task.Yield();
            
            var currentLevel = LevelController.Get().GetLevel(SceneManager.GetActiveScene().name);
            
            if (currentLevel.isGameplayLevel)
                GameplayEventChannel.PublishStart();
            else
                GameplayEventChannel.PublishEnd();  
        }

        private static void CreatePlayerAt(Vector3 position)
        {
            var player = Resources.Load<Transform>("TempPlayer");
            player = Object.Instantiate(player);
            player.position = position;
        }
    }
}