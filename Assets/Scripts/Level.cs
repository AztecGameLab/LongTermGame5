using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    // TODO replace with better scene selection method than using the buildID
    
    [SerializeField] public int buildId;
    [SerializeField] public Level[] neighbors;
}