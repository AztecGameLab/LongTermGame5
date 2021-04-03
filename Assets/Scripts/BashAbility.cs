using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BashAbility : MonoBehaviour
{
    private PlatformerController _platformerController;
    private GameInputs _inputs;
    
    private void Start()
    {
        _platformerController = GetComponentInParent<PlatformerController>();
        _inputs = _platformerController.Inputs;
        
        _inputs.Player.Bash.started += PlayerBashStart;
    }

    private void PlayerBashStart(InputAction.CallbackContext context)
    {
        var nearestBashable = Scanner.GetClosestObject<IBashable>(_platformerController.transform.position);

        if (CanBash(nearestBashable))
        {
            nearestBashable.Bash(_platformerController);
            
            // make better way to shake camera
            if (_platformerController.TryGetComponent<CinemachineImpulseSource>(out var impulseSource))
                impulseSource.GenerateImpulse();
        }
    }

    private bool CanBash(IBashable bashable)
    {
        return bashable != null  
               && _platformerController.primaryStick.normalized != Vector2.zero  
               && bashable.CanBash(_platformerController);
    }
}