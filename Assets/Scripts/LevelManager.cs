using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : Cleanup code, add option for seamless vs load screen, create better demo scene with real char
// BUGS: occasional error when touching multiple levels? 
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

    [Button]
    // Only call this method if the player is no longer touching the level to unload.
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
            Unload(level);
    }
    
    private bool ShouldBeUnloaded(Level level)
    {
        if (ActiveLevel == null)
            return false;

        if (ActiveLevel.neighbors.Contains(level))
            return false;

        return ActiveLevel != level;
    }

    [Button]
    public void LoadLevelAndNeighbors(Level level)
    {
        if (!_activeLevels.Contains(level))
            _activeLevels.Add(level);
        
        if (!level.IsLoaded())
            Load(level);

        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);

        foreach (var neighbor in level.neighbors)
            if (!_loadedLevels.Contains(neighbor))
            {
                Load(neighbor);
                _loadedLevels.Add(neighbor);
            }
    }

    private void Load(Level level)
    {
        SceneManager.LoadSceneAsync(level.buildId, LoadSceneMode.Additive);
    }
    
    private void Unload(Level level)
    {
        _loadedLevels.Remove(level);
        SceneManager.UnloadSceneAsync(level.buildId);
    }
}