using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    // TODO replace with better scene selection method
    
    [SerializeField] public int buildId;
    [SerializeField] public Level[] neighbors;

    private static readonly List<Level> LoadedLevels = new List<Level>();
    private Scene _scene;

    public bool IsLoaded()
    {
        return SceneManager.GetSceneByBuildIndex(buildId).isLoaded;
    }

    [Button]
    public void EnterScene()
    {
        if (!LoadedLevels.Contains(this))
            SceneManager.LoadScene(buildId);
        
        _scene = SceneManager.GetSceneByBuildIndex(buildId);
        Debug.Log(_scene.name);
        SceneManager.SetActiveScene(_scene);
        ClearOldLevels();

        foreach (var neighbor in neighbors)
            neighbor.Preload();
    }

    private void ClearOldLevels()
    {
        foreach (var level in LoadedLevels)
        {
            if (!neighbors.Contains(level) && level != this)
                SceneManager.UnloadSceneAsync(level.buildId);
        }
    }
    
    private void Preload()
    {
        if (LoadedLevels.Contains(this))
            return;
        
        LoadedLevels.Add(this);
        SceneManager.LoadSceneAsync(buildId, LoadSceneMode.Additive);
    }
}