using System;
using System.Collections;
using FMODUnity;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides an easy way to coordinate the Level, Transition, and Save systems.
/// </summary>
public class LevelUtil : Singleton<LevelUtil>
{
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private Level defaultLevel;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField, EventRef] private string respawnSound;
    
    private LevelController _levelController;
    private TransitionController _transitionController;
    
    private void Start()
    {
        _levelController = LevelController.Get();
        _transitionController = TransitionController.Get();
    }

    /// <summary>
    /// Smoothly transitions to a specified level.
    /// </summary>
    /// <param name="level">The level that should be loaded.</param>
    /// <param name="beforeTransitionIn">A callback that is fired after the level is loaded, and
    /// before the transition screen goes away.</param>
    /// <param name="onComplete">A callback that is fired after the full transition has completed.</param>
    public void TransitionTo(Level level, Action beforeTransitionIn = null, Action onComplete = null)
    {
        StartCoroutine(TransitionToCoroutine(level, beforeTransitionIn, onComplete));
    }

    private IEnumerator TransitionToCoroutine(Level level, Action beforeTransitionIn, Action onComplete)
    {
        _transitionController.FadeTo(Color.black, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        GC.Collect();
        _levelController.UnloadAll();
        _levelController.LoadLevel(level, beforeTransitionIn);

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == level.sceneName);
        
        
        _transitionController.FadeFrom(Color.black, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        onComplete?.Invoke();
    }

    /// <summary>
    /// Converts temporary player and scene data into a save file on disk.
    /// <remarks>Use this method over accessing the SaveLoad singleton directly.</remarks>
    /// </summary>
    public void SaveGame()
    {
        var playerData = new PlayerData
        {
            currentScene = _levelController.ActiveLevel.sceneName,
            position = PlatformerController.instance.transform.position
        };
        
        foreach (var activeLevel in _levelController.ActiveLevels)
            SaveLoad.SaveSceneToTempData(activeLevel.sceneName);

        SaveLoad.SetPlayerData(playerData);
        SaveLoad.SaveTempDataToFile();
    }

    public void LoadSavedGame()
    {
        SaveLoad.LoadFromFileToTempData();
        
        var playerData = SaveLoad.GetPlayerData();
        var level = playerData.currentScene == null ? defaultLevel : _levelController.GetLevel(playerData.currentScene);

        StartGameOnLevel(level);
    }

    public void StartGameOnLevel(Level level)
    {
        TransitionTo(level, () =>
        {
            if (level.isGameplayLevel)
                GameplayEventChannel.PublishStart();
            
            SaveLoad.LoadActiveSceneFromTempData();
            var playerData = SaveLoad.GetPlayerData();
            var playerSpawn = Vector3.zero;
            
            
            if (playerData.currentScene == null)
            {
                if (Scanner.HasObjectsInScene(out Transform[] playerSpawnTransform, o => o.CompareTag("PlayerSpawn")))
                    playerSpawn = playerSpawnTransform[0].position;
            }
            else
            {
                playerSpawn = playerData.position;
            }
            
            if (level.isGameplayLevel)
            {
                var playerGameObject = Instantiate(playerPrefab);
                playerGameObject.transform.position = playerSpawn;
                
                SaveLoad.LoadPlayerAbilites();
                RuntimeManager.PlayOneShot(respawnSound);
            }
        });        
    }
}