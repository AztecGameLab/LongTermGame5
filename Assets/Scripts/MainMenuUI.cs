using FMODUnity;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Level firstLevel;
    public Level mainMenuLevel;
    public Level creditsLevel;

    public GameObject quitButton;

    [SerializeField, EventRef] private string menuEnterSound;
    [SerializeField, EventRef] private string menuExitSound;
    [SerializeField, EventRef] private string menuHoverSound;

    private void OnLevelLoaded(Scene oldScene, Scene newScene)
    {
        // if (newScene.name == firstLevel.sceneName)
        // {
        //     print("Level " + newScene.name + " has been loaded!");
        //     
        //     var playerPrefab = Resources.Load<Transform>("TempPlayer");
        //     var playerGameObject = Instantiate(playerPrefab);
        //     var playerData = SaveLoad.GetPlayerData();
        //     playerGameObject.transform.position = playerData == null ? Vector3.zero : (Vector3) playerData.position;
        //
        //     SceneManager.activeSceneChanged -= OnLevelLoaded;
        // }
    }
    
    public void OnButtonHover()
    {
        RuntimeManager.PlayOneShot(menuHoverSound);
    }
    
    public void EnterGame()
    {
        RuntimeManager.PlayOneShot(menuEnterSound);
        
        LevelUtil.Get().LoadSavedGame();
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
