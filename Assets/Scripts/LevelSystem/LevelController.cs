using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : Singleton<LevelController>
{
    public event Action<Level, Level> ActiveLevelChanged;
    public event Action<Level> BeforeStartLoad;
    public event Action<Level> BeforeStartUnload;
    
    private LinkedList<Level> _activeLevels = new LinkedList<Level>();
    private readonly List<Level> _loadedLevels = new List<Level>();
    private readonly List<Scene> _loadingScenes = new List<Scene>();

    [SerializeField] private bool showDebugState = false;
    [SerializeField] private bool showDebugButtons = false;
    
    public IEnumerable<Level> ActiveLevels => _activeLevels;
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
        AddActiveLevel(activeLevel);
    }

    private void AddActiveLevel(Level level)
    {
        if (_activeLevels.Contains(level) || level.isPersistent) 
            return;

        var oldActiveLevel = ActiveLevel;
        _activeLevels.AddFirst(level);
        
        if (ActiveLevel == level)
            ActiveLevelChanged?.Invoke(oldActiveLevel, ActiveLevel);
    }
    
    private void RemoveActiveLevel(Level level)
    {
        if (!_activeLevels.Contains(level)) 
            return;

        var oldActiveLevel = ActiveLevel;
        _activeLevels.Remove(level);
        
        if (oldActiveLevel != ActiveLevel)
            ActiveLevelChanged?.Invoke(oldActiveLevel, ActiveLevel);
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
    
    private bool LevelShouldBeLoaded(Level level)
    {
        // Ensure that there is always one scene loaded
        if (_activeLevels.Count < 1)
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

    private void OnActiveChanged(Level oldLevel, Level newLevel)
    {
        if (newLevel == null)
            return;
        
        if (SceneManager.GetSceneByName(ActiveLevel.sceneName).isLoaded)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newLevel.sceneName));
    }

    public void LoadLevel(string sceneName, Action onLoaded = null)
    {
        LoadLevel(GetLevel(sceneName), onLoaded);
    }
    
    public void UnloadLevel(string sceneName)
    {
        UnloadLevel(GetLevel(sceneName));
    }
    
    public void LoadLevel(Level level, Action onLoaded = null)
    {
        AddActiveLevel(level);
        
        LoadAdditive(level, onLoaded);

        foreach (var neighbor in level.levelsToPreload)
            LoadAdditive(neighbor);
    }

    public void UnloadLevel(Level level)
    {
        RemoveActiveLevel(level);

        TryToUnload(level);

        foreach (var levelNeighbor in level.levelsToPreload)
            TryToUnload(levelNeighbor);
    }
    
    private void LoadAdditive(Level level, Action onLoaded = null)
    {
        if (level == null)
            return;
        
        if (!_loadedLevels.Contains(level))
        {
            BeforeStartLoad?.Invoke(level);
            
            var op = SceneManager.LoadSceneAsync(level.sceneName, LoadSceneMode.Additive);
            
            if (onLoaded != null)
                op.completed += operation => onLoaded.Invoke();

            _loadingScenes.Add(SceneManager.GetSceneByName(level.sceneName));
        }
        
        if (!_loadedLevels.Contains(level))
            _loadedLevels.Add(level);
    }
    
    private static bool IsLoaded(Level level)
    {
        return SceneManager.GetSceneByName(level.sceneName).IsValid();
    }
    
    private void TryToUnload(Level level)
    {
        if (!LevelShouldBeLoaded(level))
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
    
    public void UnloadAll()
    {
        _activeLevels.Clear();

        var levels = _loadedLevels.ToArray();
        
        foreach (var loadedLevel in levels)
        {
            if (!loadedLevel.isPersistent)
                UnloadAdditive(loadedLevel);
        }
    }

    private void OnGUI()
    {
        if (showDebugButtons)
        {
            if (GUILayout.Button("Return to menu"))
                LevelUtil.Get().TransitionTo(GetLevel("MainMenu"));

            if (GUILayout.Button("Save Game"))
                LevelUtil.Get().SaveGame();    
        }

        if (showDebugState)
        {
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
}