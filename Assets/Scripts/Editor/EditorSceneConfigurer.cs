using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class EditorSceneConfigurer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void LoadDependentScenes()
        {
            if (!SceneManager.GetSceneByName("Persistent").IsValid())
                SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);

            var playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
            
            if (playerSpawn != null)
            {
                var player = Resources.Load<Transform>("TempPlayer");
                Object.Instantiate(player);
                player.position = playerSpawn.transform.position;
            }
        }
    }
}
