using UnityEngine;

// Exposes static methods for use in UnityEvents.

[CreateAssetMenu(fileName = "Static Wrapper")]
public class StaticWrapper : ScriptableObject
{
    public static void Print(string message)
    {
        Debug.Log(message);
    }
    
    public static void SaveCurrentScene()
    {
        SaveSystem.SaveLoad.SaveCurrentScene();
    }

    public static void LoadCurrentScene()
    {
        SaveSystem.SaveLoad.LoadCurrentScene();
    }
    
    public static void LoadLevel(Level level)
    {
        LevelManager.Instance().LoadLevelAndNeighbors(level);
    }

    public static void UnloadLevel(Level level)
    {
        LevelManager.Instance().UnloadLevelAndNeighbors(level);
    }
}