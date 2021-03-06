using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorSceneConfigurer
{
    [RuntimeInitializeOnLoadMethod]
    private static void LoadDependentScenes()
    {
        if (!SceneManager.GetSceneByName("Controllers").IsValid())
        {
            SceneManager.LoadScene("Controllers", LoadSceneMode.Additive);
        }
    }
}
