using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : add option for seamless vs load screen, create better demo scene with real char
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
    
    private void Unload(Level level)
    {
        if (level.IsLoaded())
            SceneManager.UnloadSceneAsync(level.buildId);
        
        if (_loadedLevels.Contains(level))
            _loadedLevels.Remove(level);
    }

    public void LoadLevelAndNeighbors(Level level)
    {
        _activeLevels.Add(level);
        
        Load(level);

        foreach (var neighbor in level.neighbors)
            Load(neighbor);
    }

    private void Load(Level level)
    {
        if (!level.IsLoaded())
            SceneManager.LoadSceneAsync(level.buildId, LoadSceneMode.Additive);
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }

    private void OnGUI()
    {
        GUILayout.Label("Loaded Levels: ");
        foreach (var level in _loadedLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Levels: ");
        foreach (var level in _activeLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Level: " + ActiveLevel);
    }
}