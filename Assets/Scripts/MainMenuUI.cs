using System;
using FMODUnity;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene oldScene, Scene newScene)
    {
        if (newScene.name == firstLevel.sceneName)
        {
            print("Level " + newScene.name + " has been loaded!");
            
            var player = Instantiate(playerPrefab);
            var playerData = SaveLoad.GetPlayerData();
            var playerSpawn = Vector3.zero;

            if (playerData != null)
            {
                playerSpawn = playerData.position;
            }
            else if (Scanner.HasObjectsInScene(out Transform[] playerSpawnTransform, o => o.CompareTag("PlayerSpawn")))
            {
                playerSpawn = playerSpawnTransform[0].position;
            }

            player.transform.position = playerSpawn;
            SceneManager.activeSceneChanged -= OnLevelLoaded;
        }
    }
    
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
        
        SaveLoad.LoadFromFileToTempData();
        var playerData = SaveLoad.GetPlayerData();

        if (playerData == null)
        {
            // No save has been made yet, load cutscene / starting scene
            level = firstLevel;
            print("No save found: Loading " + level.sceneName);
        }
        else
        {
            level = _levelController.GetLevel(playerData.currentScene);
            firstLevel = level;
            print("Save found! Loading " + level.sceneName);
        }

        LevelUtil.Get().TransitionTo(level);
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
