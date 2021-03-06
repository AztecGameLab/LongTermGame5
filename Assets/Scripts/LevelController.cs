using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : figure out player spawning, stop level from despawning on exit
// BUGS: activeLevels starts to de-sync - haven't seen in a bit
public class LevelController : Singleton<LevelController>
{
    public event Action FinishedLoading;

    private readonly List<Level> _activeLevels = new List<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<AsyncOperation> _loadingLevels = new List<AsyncOperation>();

    [SerializeField] private bool showDebug = false;
    
    public bool Loading => _loadingLevels.Count > 0;
    private Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels[0] : null;
    private Level PreviousActiveLevel = null;
    
    private void Update()
    {
        if (PreviousActiveLevel != ActiveLevel)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(ActiveLevel.sceneName));
        
        var loadedLevels = new List<AsyncOperation>();
        var previouslyLoading = Loading;
        
        foreach (var operation in _loadingLevels)
            if (operation.isDone) loadedLevels.Add(operation);

        foreach (var operation in loadedLevels)
            _loadingLevels.Remove(operation);

        if (!Loading && previouslyLoading)
            FinishedLoading?.Invoke();
    }

    private void LateUpdate()
    {
        PreviousActiveLevel = ActiveLevel;
    }

    public void LoadLevel(Level level)
    {
        if (level.isGameplayLevel && !_activeLevels.Contains(level))
            _activeLevels.Add(level);

        LoadAdditive(level, true);

        if (level == null)
            print("Level is null");
        
        if (level.levelsToPreload == null)
            print("Levels to preload is null");
        
        foreach (var neighbor in level.levelsToPreload)
            LoadAdditive(neighbor);
    }
    
    public void UnloadLevel(Level level)
    {
        _activeLevels.Remove(level);
        
        TryToUnload(level);

        foreach (var levelNeighbor in level.levelsToPreload)
            TryToUnload(levelNeighbor);
    }

    private void TryToUnload(Level level)
    {
        if (ShouldBeUnloaded(level))
            UnloadAdditive(level);
    }
    
    private bool ShouldBeUnloaded(Level level)
    {
        if (ActiveLevel != null && ActiveLevel.levelsToPreload.Contains(level))
            return false;

        return ActiveLevel != level;
    }
    
    private void UnloadAdditive(Level level)
    {
        if (IsLoaded(level))
            _loadingLevels.Add(SceneManager.UnloadSceneAsync(level.sceneName));
        
        if (_loadedLevels.Contains(level))
            _loadedLevels.Remove(level);
    }

    private void LoadAdditive(Level level, bool setActive = false)
    {
        if (!IsLoaded(level))
            _loadingLevels.Add(SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive));
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }

    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByName(level.sceneName).IsValid();
    }

    private void OnGUI()
    {
        if (showDebug == false)
            return;
        
        GUILayout.Label("Loaded Levels: ");
        foreach (var level in _loadedLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Levels: ");
        foreach (var level in _activeLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Loading Levels");
        foreach (var level in _loadingLevels)
            GUILayout.Label("" + level.progress);
        
        GUILayout.Label("Active Level: " + ActiveLevel);
    }
}