using UnityEngine;
using static Editor.EditorUtil;

namespace Editor
{
    public static class EditorSceneSetupController
    {
        [RuntimeInitializeOnLoadMethod]
        private static void LoadDependentScenes()
        {
            EnsureSceneIsLoaded("Persistent");

            if (TryGetPlayerSpawn(out var playerSpawn))
                CreatePlayerAt(playerSpawn.transform.position);
        }

        private static void CreatePlayerAt(Vector3 position)
        {
            var player = Resources.Load<Transform>("TempPlayer");
            player = Object.Instantiate(player);
            player.position = position;
        }
    }
}