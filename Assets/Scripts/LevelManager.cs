using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        _instance = new GameObject("LevelManager", typeof(LevelManager)).GetComponent<LevelManager>();
        DontDestroyOnLoad(_instance);
    }

    private readonly List<Level> _loadedLevels = new List<Level>();
    private Level _activeLevel;
    
    [Button]
    public void TransitionTo(Level level)
    {
        _activeLevel = level;
        
        // fade out
        UnloadOldLevels();
        LoadLevelAndNeighbors();
        PositionPlayer();
        // fade in
    }

    private void UnloadOldLevels()
    {
        var removedLevels = new List<Level>();
        
        foreach (var level in _loadedLevels)
        {
            if (LevelShouldBeUnloaded(level))
            {
                SceneManager.UnloadSceneAsync(level.buildId);
                removedLevels.Add(level);
                Debug.Log("Unloaded " + level.name);
            }
        }

        foreach (var removedLevel in removedLevels)
            _loadedLevels.Remove(removedLevel);
    }

    private bool LevelShouldBeUnloaded(Level level)
    {
        return !(_activeLevel.neighbors.Contains(level) || level == _activeLevel);
    }

    private void LoadLevelAndNeighbors()
    {
        // if it isn't already loaded, load level sync
        if (!_activeLevel.IsLoaded())
            LoadLevel(_activeLevel);

        // async load level's neighbors
        foreach (var neighbor in _activeLevel.neighbors)
            if (!_loadedLevels.Contains(neighbor))
                LoadLevel(neighbor);
    }

    private void LoadLevel(Level level)
    {
        SceneManager.LoadSceneAsync(level.buildId, LoadSceneMode.Additive);
        _loadedLevels.Add(level);   
        Debug.Log("Loaded " + level.name);
    }

    private void PositionPlayer()
    {
        // find position for player in new level
        // move player to that position in the new level
    }
}