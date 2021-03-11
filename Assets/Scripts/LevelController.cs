using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : Singleton<LevelController>
{
    public event Action<Level> ActiveLevelChanged;
    public event Action<Level> BeforeStartLoad;
    public event Action<Level> BeforeStartUnload;
    
    private LinkedList<Level> _activeLevels = new LinkedList<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<Scene> _loadingScenes = new List<Scene>();

    [SerializeField] private bool showDebug = false;
    
    public bool Loading => _loadingScenes.Count > 0;
    public Level ActiveLevel => _activeLevels.Count > 0 ? _activeLevels.First.Value : null;
    private Dictionary<string, Level> _sceneNamesToLevels = new Dictionary<string, Level>();
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.sceneUnloaded += OnUnloaded;
        ActiveLevelChanged += OnActiveChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneUnloaded -= OnUnloaded;
        ActiveLevelChanged -= OnActiveChanged;
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        SetupLevelDictionary();
        SetupActiveLevel();
        SetupLoadedLevels();
    }

    private void SetupLevelDictionary()
    {
        var levels = Resources.LoadAll<Level>("Levels");
        _sceneNamesToLevels = levels.ToDictionary(level => level.sceneName);
    }
    
    public Level GetLevel(string sceneName)
    {
        return _sceneNamesToLevels[sceneName];
    }

    private void SetupActiveLevel()
    {
        var activeLevel = GetLevel(SceneManager.GetActiveScene().name);
        AddActiveLevel(activeLevel, true);
    }

    private void AddActiveLevel(Level level, bool forceActive)
    {
        if (_activeLevels.Contains(level) || level.isPersistent) 
            return;

        if (forceActive)
        {
            _activeLevels.AddFirst(level);
        }
        else
        {
            _activeLevels.AddLast(level);
        }
        
        if (ActiveLevel == level)
            ActiveLevelChanged?.Invoke(ActiveLevel);
    }
    
    private void RemoveActiveLevel(Level level)
    {
        if (!_activeLevels.Contains(level)) 
            return;

        var oldActiveLevel = ActiveLevel;
        _activeLevels.Remove(level);
        
        if (oldActiveLevel != ActiveLevel)
            ActiveLevelChanged?.Invoke(ActiveLevel);
    }

    private void SetupLoadedLevels()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var level = GetLevel(SceneManager.GetSceneAt(i).name);
            
            if (LevelShouldBeLoaded(level))
                _loadedLevels.Add(level);
        }
    }
    
    private bool LevelShouldBeLoaded(Level level, bool needsOneActive = false)
    {
        // Ensure that there is always one scene loaded
        if (needsOneActive && _activeLevels.Count < 1)
            return true;
        
        if (level.isPersistent)
            return true;
        
        // Active levels and their neighbors should always be loaded.
        foreach (var activeLevel in _activeLevels)
        {
            if (level == activeLevel)
                return true;

            if (activeLevel.levelsToPreload.Any(neighborLevel => level == neighborLevel))
                return true;
        }
        
        return false;
    }

    private void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ActiveLevel != null && ActiveLevel.sceneName == scene.name)
            SceneManager.SetActiveScene(scene);
        
        _loadingScenes.Remove(scene);
    }

    private void OnUnloaded(Scene scene)
    {
        _loadingScenes.Remove(scene);
    }

    private void OnActiveChanged(Level level)
    {
        if (level == null)
            return;
        
        if (SceneManager.GetSceneByName(ActiveLevel.sceneName).isLoaded)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(level.sceneName));
    }

    public void LoadLevel(string sceneName, bool forceActive = false)
    {
        LoadLevel(GetLevel(sceneName), forceActive);
    }
    
    public void UnloadLevel(string sceneName)
    {
        UnloadLevel(GetLevel(sceneName));
    }
    
    public void LoadLevel(Level level, bool forceActive = false)
    {
        AddActiveLevel(level, forceActive);
        
        LoadAdditive(level);

        foreach (var neighbor in level.levelsToPreload)
            LoadAdditive(neighbor);
    }

    // During gameplay, ensure that one level is always active in case the player escapes the trigger.
    public void UnloadLevel(Level level, bool needsOneActive = false)
    {
        RemoveActiveLevel(level);
        
        TryToUnload(level, needsOneActive);

        foreach (var levelNeighbor in level.levelsToPreload)
            TryToUnload(levelNeighbor, needsOneActive);
    }
    
    private void LoadAdditive(Level level)
    {
        if (level == null)
            return;
        
        if (!IsLoaded(level))
        {
            BeforeStartLoad?.Invoke(level);
            
            SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive);
            _loadingScenes.Add(SceneManager.GetSceneByName(level.sceneName));
        }
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }
    
    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByName(level.sceneName).IsValid();
    }
    
    private void TryToUnload(Level level, bool needsOneActive)
    {
        if (!LevelShouldBeLoaded(level, needsOneActive))
            UnloadAdditive(level);
    }
    
    private void UnloadAdditive(Level level)
    {
        if (level == null)
            return;
        
        if (IsLoaded(level))
        {
            BeforeStartUnload?.Invoke(level);

            SceneManager.UnloadSceneAsync(level.sceneName);
            _loadingScenes.Add(SceneManager.GetSceneByName(level.sceneName));
        }
        
        if (_loadedLevels.Contains(level))
            _loadedLevels.Remove(level);
    }
    
    public void UnloadActiveScenes()
    {
        var test = _activeLevels.ToArray();
        foreach (var activeLevel in test)
            UnloadLevel(activeLevel);
    }

    private void OnGUI()
    {
        if (showDebug == false)
            return;

        if (GUILayout.Button("Return to menu"))
        {
            UnloadActiveScenes();            
            LoadLevel("MainMenu");
        }

        if (GUILayout.Button("Save Game"))
        {
            foreach (var activeLevel in _activeLevels)
                SaveLoad.SaveSceneToTempData(activeLevel.sceneName);    
            
            var playerData = new PlayerData
            {
                currentScene = ActiveLevel.sceneName, 
                position = PlatformerController.instance.transform.position
            };

            SaveLoad.SetPlayerData(playerData);
            
            
            SaveLoad.SaveTempDataToFile();
        }
            
        GUILayout.Label("Loaded Levels: ");
        foreach (var level in _loadedLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Levels: ");
        foreach (var level in _activeLevels)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Loading Levels");
        foreach (var level in _loadingScenes)
            GUILayout.Label(level.name);
        
        GUILayout.Label("Active Level: " + ActiveLevel);
    }
}