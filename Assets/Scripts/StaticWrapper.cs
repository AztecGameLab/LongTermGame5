using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

// Exposes static methods for use in UnityEvents.

[CreateAssetMenu(fileName = "Static Wrapper")]
public class StaticWrapper : ScriptableObject
{
    public static void Print(string message)
    {
        Debug.Log(message);
    }
    
    public static void SaveActiveSceneToTempData() //for game systems
    {
        SaveSystem.SaveLoad.SaveActiveSceneToTempData(); //currently saves all loaded scenes
    }

    public static void LoadActiveSceneFromTempData() //for game systems
    {
        SaveSystem.SaveLoad.LoadActiveSceneFromTempData(); //currently loads all loaded scenes
    }
    
    public static void SaveTempDataToFile() //for when the player saves
    {
        LevelUtil.Get().SaveGame();
    }

    public static void LoadFromFileToTempData() //for when the player loads
    {
        SaveSystem.SaveLoad.LoadFromFileToTempData(); //currently loads all loaded scenes
    }
    
    public static void LoadLevel(Level level)
    {
        LevelController.Get().LoadLevel(level);
    }

    public static void UnloadLevel(Level level)
    {
        LevelController.Get().UnloadLevel(level);
    }

    public static void WaterEnemyAgro()
    {
        PassiveEnemyScript.changePassive();
    }
}