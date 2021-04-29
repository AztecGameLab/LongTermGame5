using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Editor.EditorUtil;

namespace Editor
{
    public static class EditorSceneSetupController
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity", OpenSceneMode.Additive);
                    }
                    catch
                    {
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    // User cancelled the save operation -- cancel play as well.
                    EditorApplication.isPlaying = false;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void LoadPlayerAtSpawn()
        {
            if (HasPlayerSpawn(out var playerSpawn))
                CreatePlayerAt(playerSpawn.transform.position);

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