using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour
{
    [SerializeField] private Level firstLevel;
    [SerializeField] private Level controllerLevel;
    
    private void Start()
    {
        var levelController = LevelController.Get();
        
        levelController.LoadLevel(controllerLevel);
        levelController.LoadLevel(firstLevel);

        levelController.FinishedLoading += () => SceneManager.UnloadSceneAsync(0);
    }
}