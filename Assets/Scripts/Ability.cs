using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlatformerController))]
public abstract class Ability : MonoBehaviour
{
    protected PlatformerController Player;
    protected GameInputs Inputs;
    private InputAction _inputAction;

    protected abstract string InputName { get; }

    protected virtual void OnEnable()
    {
        Player = GetComponent<PlatformerController>();
        Inputs = Player.Inputs;
        _inputAction = Inputs.Player.Get().FindAction(InputName, true);
        
        _inputAction.started += Started;
        _inputAction.canceled += Canceled;
        _inputAction.performed += Performed;
    }
    
    protected virtual void OnDisable()
    {
        _inputAction.started -= Started;
        _inputAction.canceled -= Canceled;
        _inputAction.performed -= Performed;
    }

    protected virtual void Started(InputAction.CallbackContext context) { }
    protected virtual void Canceled(InputAction.CallbackContext context) { }
    protected virtual void Performed(InputAction.CallbackContext context) { }
}