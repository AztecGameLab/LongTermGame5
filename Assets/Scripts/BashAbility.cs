using UnityEngine;

public class BashAbility : MonoBehaviour
{
    private PlatformerController _platformerController;
    private GameInputs _inputs; 
    
    private void Start()
    {
        _platformerController = GetComponentInParent<PlatformerController>();
        _inputs = new GameInputs();
        
        
    }

}
