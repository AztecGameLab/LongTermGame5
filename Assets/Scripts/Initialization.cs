using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour
{
    [SerializeField] private Level firstLevel;
    [SerializeField] private Level controllerLevel;

    private void Awake()
    {
        SceneManager.LoadScene(controllerLevel.sceneName, LoadSceneMode.Additive);
    }

    private void Start()
    {
        var levelController = LevelController.Get();

        levelController.LoadLevel(firstLevel);
        levelController.FinishedLoading += UnloadSelf;
    }

    private static void UnloadSelf()
    {
        SceneManager.UnloadSceneAsync(0);
    }
    
    private void OnDestroy()
    {
        LevelController.Get().FinishedLoading -= UnloadSelf;
    }
}