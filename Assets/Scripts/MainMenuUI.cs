using FMODUnity;
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

    [SerializeField, EventRef] private string menuEnterSound;
    [SerializeField, EventRef] private string menuExitSound;
    [SerializeField, EventRef] private string menuHoverSound;
    
    private void Start()
    {
        _levelController = LevelController.Get();
    }

    public void OnButtonHover()
    {
        RuntimeManager.PlayOneShot(menuHoverSound);
    }
    
    public void EnterGame()
    {
        RuntimeManager.PlayOneShot(menuEnterSound);
        
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
            var minimap = Minimap.Get();
            
            player.transform.position = playerPosition;
            minimap.EnableMinimap(PlatformerController.instance);
            minimap.UnlockArea(Minimap.Areas.Air);
            minimap.UnlockArea(Minimap.Areas.Bog);
            minimap.UnlockArea(Minimap.Areas.Boss);
            minimap.UnlockArea(Minimap.Areas.Lava);
            minimap.UnlockArea(Minimap.Areas.Water);
        });
    }

    public void PlayCredits()
    {
        LevelUtil.Get().TransitionTo(creditsLevel);
        RuntimeManager.PlayOneShot(menuEnterSound);
    }

    public void LoadMainMenu()
    {
        LevelUtil.Get().TransitionTo(mainMenuLevel);
        RuntimeManager.PlayOneShot(menuExitSound);
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
