using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public static class EditorSceneConfigurer
    {
        [RuntimeInitializeOnLoadMethod]
        private static async void LoadDependentScenes()
        {
            if (!SceneManager.GetSceneByName("Controllers").IsValid())
                SceneManager.LoadScene("Controllers", LoadSceneMode.Additive);

            await Task.Delay(1000);
            
            
            // LevelController.Get().RefreshLoadedScenes();
        }
    }
}
