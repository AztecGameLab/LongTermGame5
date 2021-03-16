using UnityEngine;

// Exposes static methods for use in UnityEvents.

[CreateAssetMenu(fileName = "Static Wrapper")]
public partial class StaticWrapper : ScriptableObject
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
        SaveSystem.SaveLoad.SaveTempDataToFile(); //currently saves all loaded scenes
    }

    public static void LoadFromFileToTempData() //for when the player loads
    {
        SaveSystem.SaveLoad.LoadFromFileToTempData(); //currently loads all loaded scenes
    }
}