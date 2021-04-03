using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadTrigger : Trigger
{
    [SerializeField] public Level currentLevel;

    private LevelController _levelController;
    
    private void Start()
    {
        _levelController = LevelController.Get();
        layersThatCanTrigger = 1 << LayerMask.NameToLayer("Player");
        
        events.collisionEnter.AddListener(OnLevelEnter);
        events.collisionExit.AddListener(OnLevelExit);
    }

    private void OnDisable()
    {
        events.collisionEnter.RemoveListener(OnLevelEnter);
        events.collisionExit.RemoveListener(OnLevelExit);
    }

    private void OnLevelExit(GameObject obj)
    {
        _levelController.UnloadLevel(currentLevel);
    }

    private void OnLevelEnter(GameObject obj)
    {
        _levelController.LoadLevel(currentLevel);
    }

    public void GenerateCollider()
    {
        Scene scene = SceneManager.GetActiveScene();
        var gameObjects = scene.GetRootGameObjects();

        Bounds bounds = new Bounds(gameObjects[0].transform.position, Vector3.zero);
        
        foreach (var rootObj in gameObjects)
        {
            foreach (var childObj in rootObj.GetComponentsInChildren<Renderer>())
                bounds.Encapsulate(childObj.bounds);    
        }

        colliderComponent = GenerateEdgeCollider2D(bounds);
    }

    private PolygonCollider2D GenerateEdgeCollider2D(Bounds bounds)
    {
        if (!TryGetComponent(out PolygonCollider2D edgeCollider2D))
            edgeCollider2D = gameObject.AddComponent<PolygonCollider2D>();

        var colliderSizeX = bounds.max.x - bounds.min.x;
        var colliderSizeY = bounds.max.y - bounds.min.y;

        var colliderPoints = new Vector2[4];
        colliderPoints[0] = new Vector2(-colliderSizeX / 2, colliderSizeY / 2) + (Vector2) bounds.center;
        colliderPoints[1] = new Vector2(colliderSizeX / 2, colliderSizeY / 2) + (Vector2) bounds.center;
        colliderPoints[2] = new Vector2(colliderSizeX / 2, -colliderSizeY / 2) + (Vector2) bounds.center;
        colliderPoints[3] = new Vector2(-colliderSizeX / 2, -colliderSizeY / 2) + (Vector2) bounds.center;
        
        edgeCollider2D.points = colliderPoints;
        
        return edgeCollider2D;
    }
}