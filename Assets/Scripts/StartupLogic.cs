using UnityEngine;

public class StartupLogic : MonoBehaviour
{
    [SerializeField] private Level firstLevel;
    
    private void Start()
    {
        LevelManager.Instance().LoadLevelAndNeighbors(firstLevel);
    }
}