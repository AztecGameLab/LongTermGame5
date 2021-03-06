using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    // TODO replace with better scene selection method than using the buildID
    
    [SerializeField] public string sceneName;
    [SerializeField] public Level[] neighbors;
}