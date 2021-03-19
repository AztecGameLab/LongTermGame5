using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class BashAbility : MonoBehaviour
{
    private PlatformerController _platformerController;
    private GameInputs _inputs;

    public float shakeTime = 0.5f;
    public float speedBoost = 10f;
    
    private void Start()
    {
        _platformerController = GetComponentInParent<PlatformerController>();
        _inputs = _platformerController.Inputs;
        
        _inputs.Player.Bash.started += PlayerBashStart;
    }

    private void PlayerBashStart(InputAction.CallbackContext context)
    {
        var nearestBashable = Scanner.GetClosestObject<IBashable>(_platformerController.transform.position);

        if (nearestBashable != null)
        {
            nearestBashable.Bash();

            var bashDirection = _platformerController.primaryStick;

            _platformerController.rigid.velocity = Vector2.zero;
            _platformerController.KnockBack(bashDirection, speedBoost);
        
            CameraShake.instance.StartShake(shakeTime);
            Thread.Sleep(20);   
        }
    }
}