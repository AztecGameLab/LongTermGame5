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
        }
    }
}
