using SaveSystem;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public Level firstLevel;
    public GameObject playerPrefab;
    public Level mainMenuLevel;
    public Level creditsLevel;

    public GameObject quitButton;

    private LevelController _levelController;
    
    private void Start()
    {
        _levelController = LevelController.Get();
    }

    public void EnterGame()
    {
        Level level;
        Vector3 playerPosition;
        
        SaveLoad.LoadFromFileToTempData();
        var playerData = SaveLoad.GetPlayerData();

        if (playerData == null)
        {
            // No save has been made yet, load cutscene / starting scene
            level = firstLevel;
            playerPosition = Vector3.zero;
            print("No save found: Loading " + level.sceneName);
        }
        else
        {
            level = _levelController.GetLevel(playerData.currentScene);
            playerPosition = playerData.position;
            print("Save found! Loading " + level.sceneName);
        }

        LevelUtil.Get().TransitionTo(level, () =>
        {
            var player = Instantiate(playerPrefab);
            player.transform.position = playerPosition;
        });
    }

    public void PlayCredits()
    {
        LevelUtil.Get().TransitionTo(creditsLevel);
    }

    public void LoadMainMenu()
    {
        LevelUtil.Get().TransitionTo(mainMenuLevel);
    }

    public void QuitProgram()
    {
        #if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        
        Application.Quit();

        #endif
    }

    #if UNITY_WEBGL
    private void Start()
    {
        Destroy(quitButton);
    }
    #endif
}
