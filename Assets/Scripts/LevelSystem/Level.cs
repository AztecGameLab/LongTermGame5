using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField, Tooltip("Make sure this matches the name of an actual Scene.")] 
    public string sceneName = "Init";
    
    [SerializeField, Tooltip("Levels in this list will be loaded alongside the main level, to make gameplay seamless")] 
    public Level[] levelsToPreload = new Level[0];
    
    [SerializeField, Tooltip("Can the player ever be inside this scene? (Important: used for saving)")] 
    public bool isGameplayLevel = true;

    [SerializeField, Tooltip("Should this scene remain loaded when other scenes load?")]
    public bool isPersistent = false;
}