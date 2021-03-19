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
        nearestBashable?.Bash();
    }
}