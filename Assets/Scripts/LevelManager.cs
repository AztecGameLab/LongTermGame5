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
    private readonly List<Level> _activeLevels = new List<Level>();
    private Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels[0] : null;
    
    [Button]
    public void UnloadLevelAndNeighbors(Level level)
    {
        _activeLevels.Remove(level);
        var removedLevels = new List<Level>();
        
        if (LevelShouldBeUnloaded(level))
        {
            removedLevels.Add(level);
            SceneManager.UnloadSceneAsync(level.buildId);
        }

        foreach (var levelNeighbor in level.neighbors)
        {
            if (LevelShouldBeUnloaded(levelNeighbor))
            {
                removedLevels.Add(levelNeighbor);
                SceneManager.UnloadSceneAsync(levelNeighbor.buildId);
            }
        }

        foreach (var removedLevel in removedLevels)
            _loadedLevels.Remove(removedLevel);
    }

    private bool LevelShouldBeUnloaded(Level level)
    {
        return ActiveLevel != null && !(ActiveLevel.neighbors.Contains(level) || ActiveLevel == level);
    }

    [Button]
    public void LoadLevelAndNeighbors(Level level)
    {
        if (!_activeLevels.Contains(level))
            _activeLevels.Add(level);
        
        if (!level.IsLoaded())
            LoadLevel(level);

        foreach (var neighbor in level.neighbors)
            if (!_loadedLevels.Contains(neighbor))
                LoadLevel(neighbor);
    }

    private void LoadLevel(Level level)
    {
        SceneManager.LoadSceneAsync(level.buildId, LoadSceneMode.Additive);
        _loadedLevels.Add(level);   
    }

    private void PositionPlayer()
    {
        // find position for player in new level
        // move player to that position in the new level
    }
}