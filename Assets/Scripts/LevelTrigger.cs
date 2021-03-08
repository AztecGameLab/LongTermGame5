using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public GameObject player;

    public void LoadLevel(Level level)
    {
        LevelController.Get().LoadLevel(level);
    }

    public void UnloadLevel(Level level)
    {
        LevelController.Get().UnloadLevel(level);
        SceneManager.MoveGameObjectToScene(player,
            SceneManager.GetSceneByName(LevelController.Get().ActiveLevel.sceneName));
    }
}