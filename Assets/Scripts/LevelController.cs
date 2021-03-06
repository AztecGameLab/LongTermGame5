using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : fix active scene jiggle not accurate, figure out player spawning
// BUGS: activeLevels starts to de-sync - haven't seen in a bit
public class LevelController : Singleton<LevelController>
{
    public event Action FinishedLoading;
    
    private readonly List<Level> _activeLevels = new List<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<AsyncOperation> _loadingLevels = new List<AsyncOperation>();
    
    public bool Loading => _loadingLevels.Count > 0;
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

    public void LoadLevel(Level level)
    {
        if (level.isGameplayLevel && !_activeLevels.Contains(level))
            _activeLevels.Add(level);

        StartCoroutine(LoadAdditive(level, true));

        print(level == null);
        print(level.levelsToPreload == null);
        
        foreach (var neighbor in level.levelsToPreload)
            StartCoroutine(LoadAdditive(neighbor));
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

    private IEnumerator LoadAdditive(Level level, bool setActive = false)
    {
        AsyncOperation operation = null;

        if (!IsLoaded(level))
        {
            operation = SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive);
            _loadingLevels.Add(operation);
        }
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);

        if (setActive && operation != null)
        {
            yield return new WaitUntil(() => operation.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.sceneName));
        }
        else if (setActive)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.sceneName));
        }
    }

    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByName(level.sceneName).IsValid();
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