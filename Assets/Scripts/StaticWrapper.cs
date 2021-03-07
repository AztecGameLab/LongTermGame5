using UnityEngine;

// Exposes static methods for use in UnityEvents.

[CreateAssetMenu(fileName = "Static Wrapper")]
public partial class StaticWrapper : ScriptableObject
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
}