using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : add option for seamless vs load screen
// BUGS: activeLevels starts to de-sync
public class LevelController : Singleton<LevelController>
{
    public event Action FinishedLoading;
    
    private readonly List<Level> _activeLevels = new List<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<AsyncOperation> _loadingLevels = new List<AsyncOperation>();
    
    private bool Loading => _loadingLevels.Count > 0;
    private Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels[0] : null;

    private void Update()
    {
        var loadedLevels = new List<AsyncOperation>();
        var previouslyLoading = Loading;
        
        foreach (var operation in _loadingLevels)
            if (operation.isDone) loadedLevels.Add(operation);

        foreach (var operation in loadedLevels)
            _loadingLevels.Remove(operation);

        if (!Loading && previouslyLoading)
            FinishedLoading?.Invoke();
    }

    public void LoadLevel(Level level, bool gameplayLevel = true)
    {
        if (gameplayLevel)
            _activeLevels.Add(level);
    
        LoadAdditive(level);

        foreach (var neighbor in level.neighbors)
            LoadAdditive(neighbor);
    }
    
    public void UnloadLevelAndNeighbors(Level level)
    {
        _activeLevels.Remove(level);
        
        TryToUnload(level);

        foreach (var levelNeighbor in level.neighbors)
            TryToUnload(levelNeighbor);
    }

    private void TryToUnload(Level level)
    {
        if (ShouldBeUnloaded(level))
            UnloadAdditive(level);
    }
    
    private bool ShouldBeUnloaded(Level level)
    {
        if (ActiveLevel == null)
            return false;

        if (ActiveLevel.neighbors.Contains(level))
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

    private void LoadAdditive(Level level)
    {
        if (!IsLoaded(level))
            _loadingLevels.Add(SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive));
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }

    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByName(level.sceneName).isLoaded;
    }

    private void OnGUI()
    {
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