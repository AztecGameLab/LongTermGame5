using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : add option for seamless vs load screen
// BUGS: activeLevels starts to de-sync
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance = null;
    public static LevelManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("LevelManager", typeof(LevelManager)).GetComponent<LevelManager>();
            DontDestroyOnLoad(_instance);
        }
    
        return _instance;
    }

    // This list should contain every Level that is currently loaded.
    private readonly List<Level> _loadedLevels = new List<Level>();
    
    // This list should contain every Level that the player is currently touching, ordered from oldest to newest.
    private readonly List<Level> _activeLevels = new List<Level>();
    
    // The Level that the player has been touching for the longest, if one exists.
    private Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels[0] : null;
    
    private bool _transitioning = false;
    private List<AsyncOperation> _loadingLevels = new List<AsyncOperation>();
    public bool Loading => _loadingLevels.Count > 0;

    private void Update()
    {
        var loadedLevels = new List<AsyncOperation>();
        
        foreach (var operation in _loadingLevels)
            if (operation.isDone) loadedLevels.Add(operation);

        foreach (var operation in loadedLevels)
            _loadingLevels.Remove(operation);
    }

    public void TransitionTo(Level level, Fader transition)
    {
        if (!_transitioning)
            StartCoroutine(TransitionToCoroutine(level, transition));
    }

    private IEnumerator TransitionToCoroutine(Level level, Fader transition)
    {
        _transitioning = true;
        
        transition.Fade(FadeType.In);
        yield return new WaitUntil(() => transition.Transitioning == false);
        
        foreach (var activeLevel in _activeLevels)
            UnloadAdditive(activeLevel);
        
        LoadAdditive(level);
        
        yield return new WaitUntil(() => Loading == false);
        
        transition.Fade(FadeType.Out);
        yield return new WaitUntil(() => transition.Transitioning == false);
        
        _transitioning = false;
    }
    
    public void LoadLevelAndNeighbors(Level level)
    {
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
            _loadingLevels.Add(SceneManager.UnloadSceneAsync(level.buildId));
        
        if (_loadedLevels.Contains(level))
            _loadedLevels.Remove(level);
    }

    private void LoadAdditive(Level level)
    {
        if (!IsLoaded(level))
            _loadingLevels.Add(SceneManager.LoadSceneAsync(level.buildId, LoadSceneMode.Additive));
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }

    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByBuildIndex(level.buildId).isLoaded;
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