using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField, Tooltip("Make sure this matches the name of an actual Scene.")] 
    public string sceneName;
    
    [SerializeField, Tooltip("Levels in this list will be loaded alongside the main level, to make gameplay seamless")] 
    public Level[] levelsToPreload;
    
    [SerializeField, Tooltip("Should the player character be inside this scene when it is loaded?")] 
    public bool isGameplayLevel;
}