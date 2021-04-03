using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlatformerController))]
public abstract class Ability : MonoBehaviour
{
    protected PlatformerController Player;
    private GameInputs _inputs;

    protected abstract string InputName { get; }

    protected virtual void Start()
    {
        Player = GetComponent<PlatformerController>();
        _inputs = Player.Inputs;

        var inputAction = _inputs.Player.Get().FindAction(InputName, true);
        
        inputAction.started += Started;
        inputAction.canceled += Canceled;
        inputAction.performed += Performed;
    }

    protected virtual void Started(InputAction.CallbackContext context) { }
    protected virtual void Canceled(InputAction.CallbackContext context) { }
    protected virtual void Performed(InputAction.CallbackContext context) { }
}