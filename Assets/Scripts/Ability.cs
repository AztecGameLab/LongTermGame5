using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlatformerController))]
public abstract class Ability : MonoBehaviour
{
    protected PlatformerController Player;
    private GameInputs _inputs;
    private InputAction _inputAction;

    protected abstract string InputName { get; }

    private void OnEnable()
    {
        _inputAction.started += Started;
        _inputAction.canceled += Canceled;
        _inputAction.performed += Performed;
    }

    private void OnDisable()
    {
        _inputAction.started -= Started;
        _inputAction.canceled -= Canceled;
        _inputAction.performed -= Performed;
    }
    
    protected virtual void Awake()
    {
        Player = GetComponent<PlatformerController>();
        _inputs = Player.Inputs;
        _inputAction = _inputs.Player.Get().FindAction(InputName, true);
    }

    protected virtual void Started(InputAction.CallbackContext context) { }
    protected virtual void Canceled(InputAction.CallbackContext context) { }
    protected virtual void Performed(InputAction.CallbackContext context) { }
}