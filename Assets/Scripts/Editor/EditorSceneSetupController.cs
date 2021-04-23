using System.Threading.Tasks;
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

            if (HasPlayerSpawn(out var playerSpawn))
                CreatePlayerAt(playerSpawn.transform.position);
        }

        private static async Task CreatePlayerAt(Vector3 position)
        {
            var player = Resources.Load<Transform>("TempPlayer");
            player = Object.Instantiate(player);
            player.position = position;

            await Task.Yield();

            var minimap = Minimap.Get();
            minimap.EnableMinimap(player.GetComponent<PlatformerController>());
            minimap.UnlockAllAreas(); // replace with save system method eventually?!
        }
    }
}