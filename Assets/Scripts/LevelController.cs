using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : figure out player spawning, stop level from despawning on exit
// BUGS: activeLevels starts to de-sync - haven't seen in a bit (hopefully fixed?)
public class LevelController : Singleton<LevelController>
{
    public event Action FinishedLoading;

    private readonly List<Level> _activeLevels = new List<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<Scene> _loadingLevels = new List<Scene>();

    [SerializeField] private bool showDebug = false;
    
    public bool Loading => _loadingLevels.Count > 0;
    public Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels[0] : null;
    private Dictionary<string, Level> _sceneNamesToLevels = new Dictionary<string, Level>();
    
    private void Start()
    {
        var levels = Resources.LoadAll<Level>("Levels");
        _sceneNamesToLevels = levels.ToDictionary(level => level.sceneName);

        var activeLevel = GetLevel(SceneManager.GetActiveScene().name);
        if (!_activeLevels.Contains(activeLevel))
            _activeLevels.Add(activeLevel);

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var level = GetLevel(SceneManager.GetSceneAt(i).name);
            if (LevelShouldBeLoaded(level))
                _loadedLevels.Add(level);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.sceneUnloaded += OnUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneUnloaded -= OnUnloaded;
    }

    private void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        print("loaded " + scene.name);
        _loadingLevels.Remove(scene);

        if (ActiveLevel != null && ActiveLevel.sceneName == scene.name)
        {
            SceneManager.SetActiveScene(scene);
            print("set active " + scene.name);
        }
        
        if (_loadingLevels.Count < 1)
            FinishedLoading?.Invoke();
    }

    private void OnUnloaded(Scene scene)
    {
        print("unloaded " + scene.name);
        _loadingLevels.Remove(scene);

        if (_loadingLevels.Count < 1)
            FinishedLoading?.Invoke();
    }

    public Level GetLevel(string sceneName)
    {
        return _sceneNamesToLevels[sceneName];
    }

    public void LoadLevel(Level level)
    {
        if (level.shouldBecomeActive && !_activeLevels.Contains(level))
        {
            _activeLevels.Add(level);
            
            if (ActiveLevel == level && SceneManager.GetSceneByName(ActiveLevel.sceneName).isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.sceneName));
                print("set active " + level.sceneName);
            }
        }
        
        LoadAdditive(level);

        foreach (var neighbor in level.levelsToPreload)
            LoadAdditive(neighbor);
    }

    private bool LevelShouldBeLoaded(Level level)
    {
        if (_activeLevels.Count < 1)
            return true;
        
        if (level.isPersistent)
            return true;
        
        foreach (var activeLevel in _activeLevels)
        {
            if (level == activeLevel)
                return true;

            if (activeLevel.levelsToPreload.Any(neighborLevel => level == neighborLevel))
                return true;
        }
        
        return false;
    }

    public void LoadLevel(string sceneName)
    {
        LoadLevel(GetLevel(sceneName));
    }
    
    public void UnloadLevel(string sceneName)
    {
        UnloadLevel(GetLevel(sceneName));
    }
    
    public void UnloadLevel(Level level)
    {
        var oldActive = ActiveLevel;
        _activeLevels.Remove(level);

        if (ActiveLevel != null && oldActive != ActiveLevel && SceneManager.GetSceneByName(ActiveLevel.sceneName).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(ActiveLevel.sceneName));
            print("set active " + level.sceneName);
        }
        
        TryToUnload(level);

        foreach (var levelNeighbor in level.levelsToPreload)
            TryToUnload(levelNeighbor);
    }

    public void UnloadActiveScenes()
    {
        var test = _loadedLevels.ToArray();
        foreach (var activeLevel in test)
            UnloadAdditive(activeLevel);
    }

    private void TryToUnload(Level level)
    {
        if (!LevelShouldBeLoaded(level))
        {
            UnloadAdditive(level);
        }
    }
    
    private void UnloadAdditive(Level level)
    {
        if (IsLoaded(level))
        {
            SceneManager.UnloadSceneAsync(level.sceneName);
            _loadingLevels.Add(SceneManager.GetSceneByName(level.sceneName));
        }
        
        if (_loadedLevels.Contains(level))
            _loadedLevels.Remove(level);
    }

    private void LoadAdditive(Level level)
    {
        if (!IsLoaded(level))
        {
            SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive);
            _loadingLevels.Add(SceneManager.GetSceneByName(level.sceneName));
        }
        
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

        if (GUILayout.Button("Load Credits"))
        {
            UnloadActiveScenes();            
            LoadLevel("Credits");
        }
            
        GUILayout.Label("Loaded Levels: ");
        foreach (var level in _loadedLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Levels: ");
        foreach (var level in _activeLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Loading Levels");
        foreach (var level in _loadingLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Level: " + ActiveLevel);
    }
}